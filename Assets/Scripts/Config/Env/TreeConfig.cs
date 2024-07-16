using UnityEngine;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "TreeConfig", menuName = "ScrollMaster2D/Tree Configuration", order = 0)]
    public class TreeConfig : ScriptableObject
    {
        public GameObject treePrefab;
        public float heightOffset;
        public float topColliderOffset;
        public int treeHealth;
        public ItemConfig woodItem; // Item de madeira
        public ItemConfig stickItem; // Item de graveto
        public int woodAmount;
        public int stickAmount;
    }
}
