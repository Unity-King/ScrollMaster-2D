using UnityEngine;
using UnityEditor;
using ScrollMaster2D.Controllers;
using System.Linq;

[CustomEditor(typeof(InventoryController))]
public class InventoryControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InventoryController inventoryController = (InventoryController)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Inventory Items", EditorStyles.boldLabel);

        if (inventoryController.inventoryItems != null)
        {
            var groupedItems = inventoryController.inventoryItems.GroupBy(item => item.itemConfig.category)
                                                                 .OrderBy(group => group.Key);

            foreach (var group in groupedItems)
            {
                EditorGUILayout.LabelField(group.Key.ToString(), EditorStyles.boldLabel);
                foreach (var item in group)
                {
                    EditorGUILayout.LabelField($"{item.itemConfig.itemName}: {item.quantity}");
                }
            }
        }
    }
}
