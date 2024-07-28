using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "LandscapeConfig", menuName = "ScrollMaster2D/Landscape Configuration", order = 0)]
    public class LandscapeConfig : ScriptableObject
    {
        public Biome[] biomes;

        [System.Serializable]
        public struct Biome
        {
            public string name;
            public int biomeWidth;
            public int biomeHeight;

            public string grassTilemapName;
            public TileBase grassTile;
            public float grassAmplitude;
            public float mountainFrequency;

            public string rockTilemapName;
            public TileBase rockTile;
            public CaveConfig caveConfig;

            public string dirtTilemapName;
            public TileBase dirtTile;
            public DirtConfig dirtConfig;

            public string oreTilemapName;
            public OreConfig[] oreConfigs; // Permitir múltiplas configurações de minério

            public TreeSpawnConfig[] treeConfigs;
        }

        [System.Serializable]
        public struct TreeSpawnConfig
        {
            public TreeConfig treeConfig;
            public float spawnFrequency;
        }

        [System.Serializable]
        public struct CaveConfig
        {
            public float caveFrequency;
            public float caveThreshold;
            public bool enablePlayerSpaces;
            public Vector2Int playerSpaceRange;
        }

        [System.Serializable]
        public struct DirtConfig
        {
            public float spawnChance;
            public float dirtFrequency;
            public int dirtDepth;
            public int dirtRadius;
            public EnemyConfig enemyConfig;

            public float enemySpawnFrequency;
            public float enemySpawnRange;
        }

        [System.Serializable]
        public struct OreConfig
        {
            public TileBase oreTile;
            public float spawnChance;
            public float oreFrequency;
            public int oreDepth;
        }

        [System.Serializable]
        public struct EnemyConfig
        {
            public GameObject enemyPrefab;
            public float spawnChance;
            public int maxQuantity;
            public float enemySpawnOffset;
        }
    }
}
