using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace ScrollMaster2D.Controllers
{
    public class InventoryUIHelper : MonoBehaviour
    {
        public Canvas canvas;
        public Button openInventoryButtonPrefab;
        private InventoryController inventoryController;
        private Dictionary<ItemCategory, GameObject> categoryPanels = new Dictionary<ItemCategory, GameObject>();
        private GameObject inventoryPanel;

        void Start()
        {
            inventoryController = FindObjectOfType<InventoryController>();
            if (inventoryController == null)
            {
                Debug.LogError("InventoryController not found in the scene.");
                return;
            }

            if (canvas == null)
            {
                Debug.LogError("No Canvas assigned for Inventory UI.");
                return;
            }

            CreateOpenInventoryButton();
            CreateInventoryPanel();
            CreateInventoryUI();
        }

        private void CreateOpenInventoryButton()
        {
            Button openInventoryButton = Instantiate(openInventoryButtonPrefab, canvas.transform);
            openInventoryButton.GetComponentInChildren<Text>().text = "Open Inventory";
            openInventoryButton.onClick.AddListener(ToggleInventoryPanel);

            RectTransform rect = openInventoryButton.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 0);
        }

        private void ToggleInventoryPanel()
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }

        private void CreateInventoryPanel()
        {
            inventoryPanel = new GameObject("InventoryPanel");
            inventoryPanel.transform.SetParent(canvas.transform, false);
            inventoryPanel.AddComponent<VerticalLayoutGroup>().padding = new RectOffset(10, 10, 10, 10);
            inventoryPanel.GetComponent<VerticalLayoutGroup>().spacing = 5f;

            RectTransform rect = inventoryPanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 0);
            rect.sizeDelta = new Vector2(400, 600);

            inventoryPanel.SetActive(false);
        }

        private void CreateInventoryUI()
        {
            ClearInventoryUI();
            foreach (var item in inventoryController.inventoryItems)
            {
                if (!categoryPanels.ContainsKey(item.itemConfig.category))
                {
                    CreateCategoryPanel(item.itemConfig.category);
                }

                CreateInventorySlot(item, categoryPanels[item.itemConfig.category]);
            }
        }

        private void ClearInventoryUI()
        {
            foreach (var panel in categoryPanels.Values)
            {
                Destroy(panel);
            }
            categoryPanels.Clear();
        }

        private void CreateCategoryPanel(ItemCategory category)
        {
            GameObject panel = new GameObject(category.ToString() + "Panel");
            panel.transform.SetParent(inventoryPanel.transform, false);
            panel.AddComponent<VerticalLayoutGroup>().padding = new RectOffset(10, 10, 10, 10);
            panel.GetComponent<VerticalLayoutGroup>().spacing = 5f;

            RectTransform rect = panel.AddComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(200, 300);

            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(panel.transform, false);
            Text categoryLabel = labelObject.AddComponent<Text>();
            categoryLabel.text = category.ToString();
            categoryLabel.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            categoryLabel.color = Color.black;

            categoryPanels[category] = panel;
        }

        private void CreateInventorySlot(InventoryItem item, GameObject parentPanel)
        {
            GameObject slot = new GameObject("Slot");
            slot.transform.SetParent(parentPanel.transform, false);

            Image itemImage = new GameObject("ItemImage").AddComponent<Image>();
            itemImage.transform.SetParent(slot.transform, false);
            itemImage.rectTransform.anchoredPosition = new Vector2(-100, 0);
            itemImage.rectTransform.sizeDelta = new Vector2(30, 30);

            SpriteRenderer spriteRenderer = item.itemConfig.itemPrefab.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
                itemImage.sprite = spriteRenderer.sprite;

            GameObject textObject = new GameObject("ItemText");
            textObject.transform.SetParent(slot.transform, false);
            Text text = textObject.AddComponent<Text>();
            text.text = $"{item.itemConfig.itemName} x{item.quantity}";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.alignment = TextAnchor.MiddleLeft;
            text.rectTransform.anchoredPosition = new Vector2(0, 0);
            text.rectTransform.sizeDelta = new Vector2(150, 30);
            text.color = Color.black;
        }

        public void UpdateInventoryUI()
        {
            CreateInventoryUI();
        }
    }
}
