using UnityEngine;

public enum ItemCategory
{
    Wood,
    Stick,
    Food,
    Tools,
    Other
}

[CreateAssetMenu(fileName = "NewItemConfig", menuName = "ScrollMaster2D/Configs/Items/New")]
public class ItemConfig : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;
    public int quantity;
    public ItemCategory category; // Categoria do item
}
