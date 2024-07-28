using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "MiningConfig", menuName = "ScrollMaster2D/Mining Configuration", order = 1)]
    public class MiningConfig : ScriptableObject
    {
        public KeyCode activationKey = KeyCode.X;
        public KeyCode scavationKey = KeyCode.S;
        public TileMiningData[] miningDatas;

        [System.Serializable]
        public struct TileMiningData
        {
            public TileBase tile;
            public ItemConfig itemConfig;
            public float miningTime;
        }
    }
}
