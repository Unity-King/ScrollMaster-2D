using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ScrollMaster2D.Controllers;

public class InventoryPanelController : MonoBehaviour
{
    public InventoryController inventoryController;
    public GameObject buttonPrefab; // Prefab do bot�o do invent�rio
    public Transform buttonContainer; // O Container que possui o HorizontalLayoutGroup

    private List<GameObject> currentButtons = new List<GameObject>();

    void Start()
    {
        if (inventoryController != null)
        {
            UpdateInventoryUI();
        }
    }

    void Update()
    {
        if (inventoryController != null && inventoryController.HasInventoryChanged())
        {
            UpdateInventoryUI();
        }
    }

    void UpdateInventoryUI()
    {
        // Limpa os bot�es atuais
        foreach (var button in currentButtons)
        {
            Destroy(button);
        }
        currentButtons.Clear();

        // Cria novos bot�es para cada item no invent�rio
        for (int i = 0; i < inventoryController.inventoryItems.Count; i++)
        {
            var item = inventoryController.inventoryItems[i];
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            newButton.GetComponentInChildren<Text>().text = $"{i + 1}. {item.itemConfig.itemName}";
            currentButtons.Add(newButton);
        }
    }
}
