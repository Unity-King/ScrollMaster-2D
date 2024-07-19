using UnityEngine;
using UnityEngine.Tilemaps;
using ScrollMaster2D.Config;
using System.Collections.Generic;

namespace ScrollMaster2D.Controllers
{
    public class TerrainController : MonoBehaviour
    {
        public Tilemap grassTilemap;
        public Tilemap rockTilemap;
        public Tilemap dirtTilemap;
        public Tilemap oreTilemap;
        public LandscapeConfig landscapeConfig;
        public Transform treesParent;
        public Transform enemiesParent;

        private Dictionary<Vector2Int, List<GameObject>> spawnedEnemies = new Dictionary<Vector2Int, List<GameObject>>();

        void Start()
        {
            EnsureColliders();
            RegenerateTerrain();
        }

        void EnsureColliders()
        {
            EnsureCollider(grassTilemap);
            EnsureCollider(rockTilemap);
            EnsureCollider(dirtTilemap);
        }

        void EnsureCollider(Tilemap tilemap)
        {
            if (tilemap != null && tilemap.GetComponent<TilemapCollider2D>() == null)
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
            grassTilemap.ClearAllTiles();
            rockTilemap.ClearAllTiles();
            dirtTilemap.ClearAllTiles();
            oreTilemap.ClearAllTiles(); // Limpar o tilemap de minérios também
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
                    float height = Mathf.PerlinNoise(x * biome.mountainFrequency, 0) * biome.grassAmplitude;
                    int finalHeight = Mathf.FloorToInt(height) + biome.biomeHeight;

                    GenerateColumn(x, finalHeight, biome);
                    GenerateDirt(x, finalHeight, biome);
                    GenerateOres(x, finalHeight, biome);

                    if (!IsDirtArea(x, biome))
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
                Tilemap targetTilemap = GetTilemapToUse(tileToPlace, biome);

                if (targetTilemap != null)
                {
                    targetTilemap.SetTile(tilePosition, tileToPlace);
                }
            }
        }

        void GenerateDirt(int x, int finalHeight, LandscapeConfig.Biome biome)
        {
            int baseHeight = finalHeight - biome.dirtConfig.dirtDepth;

            if (Random.value <= biome.dirtConfig.spawnChance)
            {
                int dirtCenterY = Random.Range(baseHeight, finalHeight);

                for (int dy = -biome.dirtConfig.dirtRadius; dy <= biome.dirtConfig.dirtRadius; dy++)
                {
                    int yPos = dirtCenterY + dy;
                    if (yPos >= biome.biomeHeight && yPos < finalHeight)
                    {
                        float noiseValue = Mathf.PerlinNoise(x * biome.dirtConfig.dirtFrequency, yPos * biome.dirtConfig.dirtFrequency);
                        if (noiseValue > 0.5)
                        {
                            Vector3Int tilePosition = new Vector3Int(x, yPos, 0);
                            if (grassTilemap.HasTile(tilePosition))
                            {
                                dirtTilemap.SetTile(tilePosition, biome.dirtTile);
                                if (Random.value <= biome.dirtConfig.enemyConfig.spawnChance)
                                {
                                    GenerateEnemies(x, yPos, biome.dirtConfig.enemyConfig);
                                }
                            }
                        }
                    }
                }
            }
        }

        Tilemap GetTilemapToUse(TileBase tile, LandscapeConfig.Biome biome)
        {
            if (tile == biome.grassTile)
            {
                return grassTilemap;
            }
            else if (tile == biome.rockTile)
            {
                return rockTilemap;
            }
            else if (tile == biome.dirtTile)
            {
                return dirtTilemap;
            }
            else
            {
                foreach (var oreConfig in biome.oreConfigs)
                {
                    if (tile == oreConfig.oreTile)
                    {
                        return oreTilemap;
                    }
                }
            }
            return null;
        }

        TileBase GetTileToPlace(int x, int y, int finalHeight, LandscapeConfig.Biome biome)
        {
            if (y >= biome.biomeHeight)
            {
                return biome.grassTile;
            }

            if (y < biome.biomeHeight)
            {
                if (biome.caveConfig.enablePlayerSpaces && IsPlayerSpace(x, y, biome))
                {
                    return null;
                }

                return Mathf.PerlinNoise(x * biome.caveConfig.caveFrequency, y * biome.caveConfig.caveFrequency) > biome.caveConfig.caveThreshold
                    ? biome.rockTile
                    : null;
            }

            return null;
        }

        bool IsPlayerSpace(int x, int y, LandscapeConfig.Biome biome)
        {
            int spaceWidth = Random.Range(biome.caveConfig.playerSpaceRange.x, biome.caveConfig.playerSpaceRange.y);
            float noiseValue = Mathf.PerlinNoise(x * biome.caveConfig.caveFrequency, y * biome.caveConfig.caveFrequency);
            return noiseValue < biome.caveConfig.caveThreshold && x % spaceWidth == 0;
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
                }
            }
        }

        void GenerateOres(int x, int finalHeight, LandscapeConfig.Biome biome)
        {
            foreach (var oreConfig in biome.oreConfigs)
            {
                for (int y = 0; y < finalHeight; y++)
                {
                    if (Random.value <= oreConfig.spawnChance && y < oreConfig.oreDepth)
                    {
                        if (Mathf.PerlinNoise(x * oreConfig.oreFrequency, y * oreConfig.oreFrequency) > 0.5)
                        {
                            Vector3Int tilePosition = new Vector3Int(x, y, 0);
                            if (!rockTilemap.HasTile(tilePosition) && !dirtTilemap.HasTile(tilePosition) && !oreTilemap.HasTile(tilePosition))
                            {
                                oreTilemap.SetTile(tilePosition, oreConfig.oreTile);
                            }
                        }
                    }
                }
            }
        }

        bool IsDirtArea(int x, LandscapeConfig.Biome biome)
        {
            return Mathf.PerlinNoise(x * biome.dirtConfig.enemySpawnFrequency, 0) > 0.5f;
        }
    }
}
