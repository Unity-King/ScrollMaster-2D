using UnityEngine;

public enum ItemCategory
{
    Wood,
    Stick,
    Foods,
    Tools,
    Golds,
    Grounds,
    Other
}

[CreateAssetMenu(fileName = "NewItemConfig", menuName = "ScrollMaster2D/Configs/Items/New")]
public class ItemConfig : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;
    public int quantity;
    public ItemCategory category; 
}
