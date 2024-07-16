using UnityEngine;

namespace ScrollMaster2D.Controllers
{
    public class ItemPickUpController : MonoBehaviour
    {
        public ItemConfig itemConfig; // Configuração do item

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                InventoryController inventory = other.GetComponent<InventoryController>();
                if (inventory != null)
                {
                    inventory.AddItem(itemConfig.itemName, itemConfig.quantity);
                    Debug.Log($"Picked up {itemConfig.quantity} {itemConfig.itemName}(s).");
                    Destroy(gameObject); // Destroi o item após ser coletado
                }
            }
        }
    }
}
