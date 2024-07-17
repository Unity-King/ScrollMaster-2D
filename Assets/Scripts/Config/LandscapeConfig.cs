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
            public CaveConfig caveConfig; // Configura��o das cavernas
            public EnemyAreaConfig enemyAreaConfig; // Configura��o da �rea inimiga
        }

        [System.Serializable]
        public struct TreeSpawnConfig
        {
            public TreeConfig treeConfig;
            public float spawnFrequency; // Frequ�ncia de spawn das �rvores
        }

        [System.Serializable]
        public struct CaveConfig
        {
            public float caveFrequency; // Frequ�ncia do ru�do para gera��o de cavernas
            public float caveThreshold; // Limite para determinar se um tile � parte de uma caverna
        }

        [System.Serializable]
        public struct EnemyAreaConfig
        {
            public float areaFrequency; // Frequ�ncia da �rea inimiga no bioma
            public TileBase enemyTile; // Tile da �rea inimiga
            public EnemyConfig enemyConfig; // Configura��o dos inimigos
            public bool allowTreeSpawn; // Permitir ou n�o o spawn de �rvores
        }

        [System.Serializable]
        public struct EnemyConfig
        {
            public GameObject enemyPrefab; // Prefab do inimigo
            public float spawnChance; // Chance de spawn
            public int maxQuantity; // Quantidade m�xima de inimigos
            public float enemySpawnOffset; // Dist�ncia m�nima entre inimigos
        }
    }
}
