using UnityEngine;
using ScrollMaster2D.Config;
using System.Collections;

namespace ScrollMaster2D.Controllers
{
    public class ChestController : MonoBehaviour
    {
        public ChestConfig chestConfig;
        public KeyCode openKey = KeyCode.E;
        public float interactionRange = 2.0f;

        private Animator animator;
        private Transform playerTransform;
        private bool isOpen = false;
        private bool isPlayerInRange = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
            }
        }

        void Update()
        {
            if (playerTransform != null)
            {
                if (IsPlayerInRange() && !isOpen)
                {
                    if (Input.GetKeyDown(openKey))
                    {
                        OpenChest();
                    }
                }
            }
        }

        private bool IsPlayerInRange()
        {
            return Vector3.Distance(transform.position, playerTransform.position) <= interactionRange;
        }

        private void OpenChest()
        {
            isOpen = true;
            animator.SetBool("isOpen", true);

            DropLoot();
            GrantExperience();
            TrySpawnMonster();
        }

        private void DropLoot()
        {
            foreach (var itemConfig in chestConfig.lootItems)
            {
                Instantiate(itemConfig.itemPrefab, transform.position, Quaternion.identity);
            }
        }

        private void GrantExperience()
        {
            if (chestConfig.expAmount > 0)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    Exp playerExp = player.GetComponent<Exp>();
                    if (playerExp != null)
                    {
                        playerExp.AddExperience(chestConfig.expAmount);
                    }
                }
            }
        }

        private void TrySpawnMonster()
        {
            if (chestConfig.canSpawnMonster && chestConfig.spawnableMonster != null)
            {
                Instantiate(chestConfig.spawnableMonster, transform.position, Quaternion.identity);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
