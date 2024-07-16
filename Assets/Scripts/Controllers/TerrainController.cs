using UnityEngine;
using UnityEngine.Tilemaps;
using ScrollMaster2D.Config;

namespace ScrollMaster2D.Controllers
{
    public class ProceduralGeneration : MonoBehaviour
    {
        public Tilemap tilemap;
        public LandscapeConfig landscapeConfig; // Single LandscapeConfig ScriptableObject
        public TreeConfig[] treeConfigs;
        public Transform treesParent;
        public GameObject chestPrefab; // Prefab for the chest
        public float treeSpacing = 5.0f;
        private System.Random random = new System.Random();

        void Start()
        {
            EnsureCollider();
            GenerateTerrain();
            GenerateTrees();
            PlaceChests();
        }

        void EnsureCollider()
        {
            TilemapCollider2D tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();
            if (tilemapCollider == null)
            {
                tilemapCollider = tilemap.gameObject.AddComponent<TilemapCollider2D>();
            }

            Rigidbody2D rb = tilemap.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = tilemap.gameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        void GenerateTerrain()
        {
            // Generate base grass terrain
            for (int x = 0; x < landscapeConfig.mapWidth; x++)
            {
                for (int y = 0; y < landscapeConfig.grassHeight; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    tilemap.SetTile(tilePosition, landscapeConfig.grassTile);
                }
            }

            // Generate terrain features
            foreach (var feature in landscapeConfig.terrainFeatures)
            {
                foreach (var offset in feature.shape)
                {
                    Vector3Int tilePosition = new Vector3Int(feature.startPosition.x + offset.x, feature.startPosition.y + offset.y, 0);
                    tilemap.SetTile(tilePosition, feature.tile);
                }
            }

            GenerateHighAreas();
            GenerateEnemyAreas();
        }

        void GenerateHighAreas()
        {
            foreach (var pos in landscapeConfig.highAreaPositions)
            {
                Vector3Int tilePosition = new Vector3Int(pos.x, pos.y, 0);
                tilemap.SetTile(tilePosition, landscapeConfig.grassTile); // You can use a different tile for high areas if needed
            }
        }

        void GenerateEnemyAreas()
        {
            foreach (var pos in landscapeConfig.enemyAreaPositions)
            {
                // Just mark the enemy area for now
                Debug.Log($"Enemy area at: ({pos.x}, {pos.y})");
            }
        }

        void GenerateTrees()
        {
            if (treesParent == null)
            {
                GameObject newParent = new GameObject("TreesParent");
                treesParent = newParent.transform;
            }

            float currentX = 0;
            while (currentX < landscapeConfig.mapWidth)
            {
                TreeConfig config = treeConfigs[random.Next(treeConfigs.Length)];
                Vector3 treePosition = new Vector3(currentX, landscapeConfig.grassHeight + config.heightOffset + config.topColliderOffset, 0);
                GameObject tree = Instantiate(config.treePrefab, treePosition, Quaternion.identity, treesParent);

                // Log tree properties for debugging purposes
                Debug.Log($"Tree placed with {config.woodAmount} wood, {config.stickAmount} sticks, and {config.treeHealth} health.");

                currentX += treeSpacing;
            }
        }

        void PlaceChests()
        {
            Vector2Int chestPos = landscapeConfig.highAreaPositions[random.Next(landscapeConfig.highAreaPositions.Length)];
            Vector3 chestPosition = new Vector3(chestPos.x, chestPos.y + 1, 0); // Place chest on higher ground
            Instantiate(chestPrefab, chestPosition, Quaternion.identity);
        }
    }
}
