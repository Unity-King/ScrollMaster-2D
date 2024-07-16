using UnityEngine;
using UnityEngine.UI;
using ScrollMaster2D.Config;
using System.Collections;

namespace ScrollMaster2D.Controllers
{
    public class TreeController : MonoBehaviour
    {
        public TreeConfig treeConfig; // Configuração da árvore
        public KeyCode collectKey = KeyCode.E; // Tecla para coletar recursos
        public float interactionRange = 2.0f; // Alcance de interação configurável
        public float bounceDuration = 0.2f; // Duração do efeito de bounce
        public float bounceScale = 1.2f; // Escala do efeito de bounce

        public int currentHealth;
        private Transform playerTransform;
        private GameObject collectButton;
        private Vector3 originalScale;

        void Start()
        {
            if (treeConfig != null)
            {
                currentHealth = treeConfig.treeHealth;
            }

            // Localizar o botão de coleta pela tag
            collectButton = GameObject.FindWithTag("CollectButton");
            if (collectButton != null)
            {
                collectButton.SetActive(false); // Esconde o botão de coleta inicialmente
            }
            else
            {
                Debug.LogError("Collect button not found. Make sure the button exists in the scene with the 'CollectButton' tag.");
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
                Debug.Log("Player found and assigned.");
            }
            else
            {
                Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
            }

            originalScale = transform.localScale;
        }

        void Update()
        {
            if (playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                Debug.Log($"Distance to player: {distance}"); // Log the distance for debugging

                if (distance <= interactionRange)
                {
                    if (collectButton != null && !collectButton.activeSelf)
                    {
                        collectButton.SetActive(true); // Mostra o botão de coleta quando o jogador está perto
                        Debug.Log("Player in range, showing collect button");
                    }

                    if (Input.GetKeyDown(collectKey))
                    {
                        CollectResources();
                    }
                }
                else
                {
                    if (collectButton != null && collectButton.activeSelf)
                    {
                        collectButton.SetActive(false); // Esconde o botão de coleta quando o jogador sai de perto
                        Debug.Log("Player out of range, hiding collect button");
                    }
                }
            }
            else
            {
                Debug.LogError("Player transform is null. Make sure the player object is correctly tagged and found.");
            }
        }

        private void CollectResources()
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= interactionRange)
            {
                currentHealth--;

                DropResources();
                StartCoroutine(BounceTree());

                if (currentHealth <= 0)
                {
                    Destroy(gameObject); // Destroi a árvore quando sua vida chega a zero
                }
            }
            else
            {
                Debug.LogWarning($"Player tried to collect resources but is out of range. Distance: {distance}");
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

        // Método para desenhar o Gizmo no Editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
