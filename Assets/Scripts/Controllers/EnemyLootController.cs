using UnityEngine;
using ScrollMaster2D.Config;
using ScrollMaster2D.Managers;

namespace ScrollMaster2D.Controllers
{
    public class EnemyLootController : MonoBehaviour
    {
        public void DropLoot(EnemyConfig enemyConfig)
        {
            foreach (var lootItem in enemyConfig.lootItems)
            {
                if (Random.value <= lootItem.dropChance)
                {
                    int quantity = Random.Range(lootItem.minQuantity, lootItem.maxQuantity + 1);
                    for (int i = 0; i < quantity; i++)
                    {
                        Instantiate(lootItem.itemConfig.itemPrefab, transform.position, Quaternion.identity);
                    }

                    Debug.Log($"Dropped {quantity} of {lootItem.itemConfig.itemName}");
                }
            }
        }
    }
}
