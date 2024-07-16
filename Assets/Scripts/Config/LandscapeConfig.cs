using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScrollMaster2D.Config
{
    [CreateAssetMenu(fileName = "LandscapeConfig", menuName = "ScrollMaster2D/Landscape Configuration", order = 0)]
    public class LandscapeConfig : ScriptableObject
    {
        public int mapWidth = 50;
        public int mapHeight = 10;
        public int grassHeight = 3;
        public TileBase grassTile;
        public Vector2Int[] highAreaPositions; // Positions of higher ground areas
        public Vector2Int[] enemyAreaPositions; // Positions designated for enemy areas
        public TerrainFeature[] terrainFeatures; // Array of terrain features for complex structures

        [System.Serializable]
        public struct TerrainFeature
        {
            public Vector2Int startPosition; // Starting position of the feature
            public Vector2Int[] shape; // Shape of the feature
            public TileBase tile; // Tile to use for the feature
        }
    }
}
