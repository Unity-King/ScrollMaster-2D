using UnityEngine;
using System;
using System.Collections;
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class TreeController : MonoBehaviour
    {
        public TreeConfig treeConfig;
        public KeyCode collectKey = KeyCode.E;
        public float interactionRange = 2.0f;
        public float bounceDuration = 0.2f;
        public float bounceScale = 1.2f;

        public int currentHealth;
        private Transform playerTransform;
        private static TreeController currentInteractableTree;
        private Vector3 originalScale;

        public static event Action<TreeController> OnTreeInRange;
        public static event Action OnTreeOutOfRange;

        void Start()
        {
            if (treeConfig != null)
            {
                currentHealth = treeConfig.treeHealth;
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
            }

            originalScale = transform.localScale;
        }

        void Update()
        {
            if (playerTransform == null) return;

            if (IsPlayerInRange())
            {
                if (currentInteractableTree != this)
                {
                    currentInteractableTree?.NotifyOutOfRange();
                    currentInteractableTree = this;
                    OnTreeInRange?.Invoke(this);
                }

                if (Input.GetKeyDown(collectKey))
                {
                    CollectResources();
                }
            }
            else if (currentInteractableTree == this)
            {
                NotifyOutOfRange();
            }
        }

        private bool IsPlayerInRange()
        {
            return Vector3.Distance(transform.position, playerTransform.position) <= interactionRange;
        }

        public void CollectResources()
        {
            if (!IsPlayerInRange()) return;

            currentHealth--;

            DropResources();
            StartCoroutine(BounceTree());

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                NotifyOutOfRange();
            }
        }

        private IEnumerator BounceTree()
        {
            Vector3 targetScale = originalScale * bounceScale;
            float time = 0;

            while (time < bounceDuration)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, time / bounceDuration);
                time += Time.deltaTime;
                yield return null;
            }

            transform.localScale = originalScale;
        }

        private void DropResources()
        {
            Debug.Log("Dropping resources...");

            if (treeConfig.woodItem != null)
            {
                DropItem(treeConfig.woodItem, treeConfig.woodAmount);
            }

            if (treeConfig.stickItem != null)
            {
                DropItem(treeConfig.stickItem, treeConfig.stickAmount);
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                InventoryController playerInventory = player.GetComponent<InventoryController>();
                if (playerInventory != null)
                {
                    playerInventory.AddItem(treeConfig.woodItem.itemName, treeConfig.woodAmount);
                    playerInventory.AddItem(treeConfig.stickItem.itemName, treeConfig.stickAmount);
                }
            }
        }

 

        private void DropItem(ItemConfig itemConfig, int totalAmount)
        {
            int amountPerDrop = Mathf.Max(1, totalAmount / treeConfig.treeHealth);
            for (int i = 0; i < amountPerDrop; i++)
            {
                Instantiate(itemConfig.itemPrefab, transform.position, Quaternion.identity);
            }
        }

        private void NotifyOutOfRange()
        {
            if (currentInteractableTree == this)
            {
                OnTreeOutOfRange?.Invoke();
                currentInteractableTree = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
