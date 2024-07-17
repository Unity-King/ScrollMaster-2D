using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "MiningConfig", menuName = "ScrollMaster2D/Mining Configuration", order = 1)]
    public class MiningConfig : ScriptableObject
    {
        public KeyCode activationKey = KeyCode.X;  // Key to toggle mining mode
        public KeyCode scavationKey = KeyCode.S;   // Key to initiate scavation mode
        public TileMiningData[] miningDatas;

        [System.Serializable]
        public struct TileMiningData
        {
            public TileBase tile;
            public ItemConfig itemConfig;  // Configuration for the item to be dropped
            public float miningTime;       // Time needed to completely mine
        }

       
    }
}
