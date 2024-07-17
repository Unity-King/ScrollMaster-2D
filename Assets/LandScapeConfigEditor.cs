using UnityEngine;
using UnityEditor;
using ScrollMaster2D.Controllers;

[CustomEditor(typeof(TerrainController))]
public class TerrainControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainController terrainController = (TerrainController)target;
        if (GUILayout.Button("Regenerate Terrain"))
        {
            terrainController.RegenerateTerrain();
        }
    }
}
