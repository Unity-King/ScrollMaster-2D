using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "GreenmountLandscapeConfig", menuName = "ScrollMaster2D/Greenmount Landscape Configuration", order = 1)]
    public class GreenmountLandscapeConfig : LandscapeConfig
    {
        public GreenmountLandscapeConfig()
        {
            biomes = new Biome[]
            {
                new Biome
                {
                    name = "Greenmount",
                    biomeWidth = 100,
                    grassHeight = 4,
                    primaryTile = Resources.Load<TileBase>("Tiles/GrassTile"), // Ensure to set your tile paths correctly
                    secondaryTile = Resources.Load<TileBase>("Tiles/RockTile"),
                    mountainAmplitude = 2.0f,
                    mountainFrequency = 0.05f,
                    treeConfigs = new TreeSpawnConfig[]
                    {
                        new TreeSpawnConfig
                        {
                            treeConfig = new TreeConfig
                            {
                                treePrefab = Resources.Load<GameObject>("Prefabs/TreePrefab"), // Ensure to set your prefab paths correctly
                                heightOffset = 0.5f
                            },
                            spawnFrequency = 0.1f
                        }
                    },
                    caveConfig = new CaveConfig
                    {
                        caveFrequency = 0.1f,
                        caveThreshold = 0.5f
                    },
                    enemyAreaConfig = new EnemyAreaConfig
                    {
                        areaFrequency = 0.1f,
                        enemyTile = Resources.Load<TileBase>("Tiles/EnemyTile"), // Ensure to set your tile paths correctly
                        enemyConfig = new EnemyConfig
                        {
                            enemyPrefab = Resources.Load<GameObject>("Prefabs/EnemyPrefab"), // Ensure to set your prefab paths correctly
                            spawnChance = 0.2f,
                            maxQuantity = 3,
                            enemySpawnOffset = 1.5f
                        },
                        allowTreeSpawn = false
                    }
                }
            };
        }
    }
}
