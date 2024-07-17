using UnityEngine;
using UnityEngine.Tilemaps;
using ScrollMaster2D.Config;
using System.Collections.Generic;

namespace ScrollMaster2D.Controllers
{
    public class TerrainController : MonoBehaviour
    {
        public Tilemap tilemap;
        public LandscapeConfig landscapeConfig;
        public Transform treesParent;
        public Transform enemiesParent;

        private Dictionary<Vector2Int, List<GameObject>> spawnedEnemies = new Dictionary<Vector2Int, List<GameObject>>();

        void Start()
        {
            EnsureCollider();
            RegenerateTerrain();
        }

        void EnsureCollider()
        {
            if (tilemap.GetComponent<TilemapCollider2D>() == null)
            {
                var collider = tilemap.gameObject.AddComponent<TilemapCollider2D>();
                var rb = tilemap.gameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        public void RegenerateTerrain()
        {
            ClearExistingObjects();
            GenerateTerrainAndTrees();
        }

        void ClearExistingObjects()
        {
            tilemap.ClearAllTiles();
            ClearChildren(treesParent);
            ClearChildren(enemiesParent);
            spawnedEnemies.Clear();
        }

        void ClearChildren(Transform parent)
        {
            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(child.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        void GenerateTerrainAndTrees()
        {
            int currentX = 0;

            foreach (var biome in landscapeConfig.biomes)
            {
                for (int x = currentX; x < currentX + biome.biomeWidth; x++)
                {
                    float height = Mathf.PerlinNoise(x * biome.mountainFrequency, 0) * biome.mountainAmplitude;
                    int finalHeight = Mathf.FloorToInt(height) + biome.grassHeight;

                    GenerateColumn(x, finalHeight, biome);

                    if (!IsEnemyArea(x, biome))
                    {
                        GenerateTrees(x, finalHeight, biome);
                    }
                }
                currentX += biome.biomeWidth;
            }
        }

        void GenerateColumn(int x, int finalHeight, LandscapeConfig.Biome biome)
        {
            for (int y = 0; y < finalHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tileToPlace = GetTileToPlace(x, y, finalHeight, biome);
                tilemap.SetTile(tilePosition, tileToPlace);
            }
        }

        TileBase GetTileToPlace(int x, int y, int finalHeight, LandscapeConfig.Biome biome)
        {
            if (y < biome.grassHeight)
            {
                return Mathf.PerlinNoise(x * biome.caveConfig.caveFrequency, y * biome.caveConfig.caveFrequency) > biome.caveConfig.caveThreshold
                    ? biome.secondaryTile
                    : biome.primaryTile;
            }

            if (Mathf.PerlinNoise(x * biome.enemyAreaConfig.areaFrequency, 0) > 0.5f)
            {
                GenerateEnemies(x, finalHeight, biome.enemyAreaConfig.enemyConfig);
                return biome.enemyAreaConfig.enemyTile;
            }

            return biome.primaryTile;
        }

        void GenerateTrees(int x, int terrainHeight, LandscapeConfig.Biome biome)
        {
            if (treesParent == null)
            {
                treesParent = new GameObject("TreesParent").transform;
            }

            foreach (var treeConfig in biome.treeConfigs)
            {
                if (Random.value <= treeConfig.spawnFrequency)
                {
                    Vector3 treePosition = new Vector3(x, terrainHeight + treeConfig.treeConfig.heightOffset, 0);
                    Instantiate(treeConfig.treeConfig.treePrefab, treePosition, Quaternion.identity, treesParent);
                    Debug.Log($"Tree spawned at ({x}, {terrainHeight}) with prefab {treeConfig.treeConfig.treePrefab.name}.");
                }
            }
        }

        void GenerateEnemies(int x, int terrainHeight, LandscapeConfig.EnemyConfig enemyConfig)
        {
            if (enemiesParent == null)
            {
                enemiesParent = new GameObject("EnemiesParent").transform;
            }

            Vector2Int positionKey = new Vector2Int(x, terrainHeight);

            if (!spawnedEnemies.ContainsKey(positionKey))
            {
                spawnedEnemies[positionKey] = new List<GameObject>();
            }

            int quantity = Random.Range(1, enemyConfig.maxQuantity + 1);
            for (int i = 0; i < quantity; i++)
            {
                if (Random.value <= enemyConfig.spawnChance)
                {
                    Vector3 offset = new Vector3(Random.Range(-enemyConfig.enemySpawnOffset, enemyConfig.enemySpawnOffset), 0, 0);
                    Vector3 enemyPosition = new Vector3(x, terrainHeight + 1, 0) + offset;
                    GameObject enemy = Instantiate(enemyConfig.enemyPrefab, enemyPosition, Quaternion.identity, enemiesParent);
                    spawnedEnemies[positionKey].Add(enemy);
                    Debug.Log($"Enemy spawned at ({enemyPosition.x}, {enemyPosition.y}) with prefab {enemyConfig.enemyPrefab.name}.");
                }
            }
        }

        bool IsEnemyArea(int x, LandscapeConfig.Biome biome)
        {
            return Mathf.PerlinNoise(x * biome.enemyAreaConfig.areaFrequency, 0) > 0.5f;
        }
    }
}

