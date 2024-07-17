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
            public int biomeWidth; // Largura do bioma
            public int grassHeight;
            public TileBase primaryTile;
            public TileBase secondaryTile;
            public float mountainAmplitude;
            public float mountainFrequency;
            public TreeSpawnConfig[] treeConfigs;
            public CaveConfig caveConfig; // Configuração das cavernas
            public EnemyAreaConfig enemyAreaConfig; // Configuração da área inimiga
        }

        [System.Serializable]
        public struct TreeSpawnConfig
        {
            public TreeConfig treeConfig;
            public float spawnFrequency; // Frequência de spawn das árvores
        }

        [System.Serializable]
        public struct CaveConfig
        {
            public float caveFrequency; // Frequência do ruído para geração de cavernas
            public float caveThreshold; // Limite para determinar se um tile é parte de uma caverna
        }

        [System.Serializable]
        public struct EnemyAreaConfig
        {
            public float areaFrequency; // Frequência da área inimiga no bioma
            public TileBase enemyTile; // Tile da área inimiga
            public EnemyConfig enemyConfig; // Configuração dos inimigos
            public bool allowTreeSpawn; // Permitir ou não o spawn de árvores
        }

        [System.Serializable]
        public struct EnemyConfig
        {
            public GameObject enemyPrefab; // Prefab do inimigo
            public float spawnChance; // Chance de spawn
            public int maxQuantity; // Quantidade máxima de inimigos
            public float enemySpawnOffset; // Distância mínima entre inimigos
        }
    }
}
