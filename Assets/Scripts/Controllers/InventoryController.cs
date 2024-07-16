using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor; 
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class InventoryController : MonoBehaviour
    {
        public Character characterConfig; 
        [SerializeField] public List<InventoryItem> inventoryItems = new List<InventoryItem>();

        private Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        private Dictionary<string, int> previousItemCounts = new Dictionary<string, int>();

        void Update()
        {
            if (HasInventoryChanged())
            {
                UpdateInventoryItems();
                EditorUtility.SetDirty(this);
            }
        }

        public void AddItem(string itemName, int amount)
        {
            if (itemCounts.ContainsKey(itemName))
            {
                itemCounts[itemName] += amount;
            }
            else
            {
                itemCounts[itemName] = amount;
            }

            UpdateInventoryItems();
            EditorUtility.SetDirty(this); 
        }

        public int GetItemCount(string itemName)
        {
            return itemCounts.ContainsKey(itemName) ? itemCounts[itemName] : 0;
        }

        private bool HasInventoryChanged()
        {
            if (previousItemCounts.Count != itemCounts.Count)
            {
                return true;
            }

            foreach (var item in itemCounts)
            {
                if (!previousItemCounts.ContainsKey(item.Key) || previousItemCounts[item.Key] != item.Value)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateInventoryItems()
        {
            inventoryItems.Clear();
            foreach (var item in itemCounts)
            {
                var config = ItemDatabase.GetItemConfig(item.Key);
                if (config != null)
                {
                    inventoryItems.Add(new InventoryItem
                    {
                        itemConfig = config,
                        quantity = item.Value
                    });
                }
            }

            previousItemCounts = new Dictionary<string, int>(itemCounts);
        }
    }

    [System.Serializable]
    public class InventoryItem
    {
        public ItemConfig itemConfig;
        public int quantity;
    }

    public static class ItemDatabase
    {
        private static Dictionary<string, ItemConfig> itemConfigs = new Dictionary<string, ItemConfig>();

        public static void AddItemConfig(ItemConfig config)
        {
            if (!itemConfigs.ContainsKey(config.itemName))
            {
                itemConfigs[config.itemName] = config;
            }
        }

        public static ItemConfig GetItemConfig(string itemName)
        {
            itemConfigs.TryGetValue(itemName, out ItemConfig config);
            return config;
        }
    }
}
