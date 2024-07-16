using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScrollMaster2D.Config.LandscapeConfig))]
public class LandscapeConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScrollMaster2D.Config.LandscapeConfig config = (ScrollMaster2D.Config.LandscapeConfig)target;

        if (GUILayout.Button("Add Terrain Feature"))
        {
            AddTerrainFeature(config);
        }

        EditorGUILayout.Space();

        foreach (var feature in config.terrainFeatures)
        {
            EditorGUILayout.LabelField("Feature at (" + feature.startPosition.x + ", " + feature.startPosition.y + ")");
            foreach (var pos in feature.shape)
            {
                EditorGUILayout.LabelField("  Offset: (" + pos.x + ", " + pos.y + ")");
            }
        }
    }

    private void AddTerrainFeature(ScrollMaster2D.Config.LandscapeConfig config)
    {
        ArrayUtility.Add(ref config.terrainFeatures, new ScrollMaster2D.Config.LandscapeConfig.TerrainFeature
        {
            startPosition = Vector2Int.zero,
            shape = new Vector2Int[0],
            tile = null
        });
    }
}

