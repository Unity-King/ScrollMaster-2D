using UnityEngine;
using UnityEngine.Tilemaps;
using ScrollMaster2D.Config;
using System.Collections;

namespace ScrollMaster2D.Controllers
{
    public class MiningController : MonoBehaviour
    {
        public MiningConfig miningConfig;
        public Camera mainCamera;
        public Tilemap[] tilemaps; 

        private TileBase currentHighlightedTile;
        private Vector3Int previousMousePosition;
        private Tilemap currentTilemap;
        private bool isEditorMode = false;
        private float startTime;

        void Update()
        {
            if (Input.GetKeyDown(miningConfig.activationKey))
            {
                ToggleEditMode();
            }

            if (isEditorMode)
            {
                HandleTileHighlight();

                if (Input.GetKeyDown(miningConfig.scavationKey) && currentHighlightedTile != null)
                {
                    StartScavation(previousMousePosition);
                }

                if (Input.GetKey(miningConfig.scavationKey))
                {
                    UpdateScavation(previousMousePosition);
                }
            }
        }

        private void ToggleEditMode()
        {
            isEditorMode = !isEditorMode;
            if (!isEditorMode && currentHighlightedTile != null)
            {
                currentTilemap.SetColor(previousMousePosition, Color.white);
                currentHighlightedTile = null;
            }
        }

        private void HandleTileHighlight()
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mousePos = WorldToCell(worldPos);

            if (mousePos != previousMousePosition)
            {
                if (currentHighlightedTile != null)
                {
                    currentTilemap.SetColor(previousMousePosition, Color.white); 
                }

                foreach (var tilemap in tilemaps)
                {
                    if (tilemap.HasTile(mousePos))
                    {
                        currentTilemap = tilemap;
                        currentHighlightedTile = tilemap.GetTile(mousePos);
                        tilemap.SetColor(mousePos, Color.yellow); 
                        previousMousePosition = mousePos;
                        break;
                    }
                }

                if (currentTilemap == null)
                {
                    currentHighlightedTile = null;
                }
            }
        }

        private void StartScavation(Vector3Int position)
        {
            startTime = Time.time;
        }

        private void UpdateScavation(Vector3Int position)
        {
            if (currentTilemap.GetTile(position) == currentHighlightedTile)
            {
                float progress = (Time.time - startTime) / GetMiningData(currentHighlightedTile).Value.miningTime;
                currentTilemap.SetColor(position, Color.Lerp(Color.yellow, Color.black, progress));

                if (progress >= 1.0f)
                {
                    BreakTile(position);
                }
            }
        }

        private void BreakTile(Vector3Int tilePosition)
        {
            var miningData = GetMiningData(currentTilemap.GetTile(tilePosition));
            if (miningData != null)
            {
                currentTilemap.SetTile(tilePosition, null);  
                Instantiate(miningData.Value.itemConfig.itemPrefab, currentTilemap.GetCellCenterWorld(tilePosition), Quaternion.identity);  
                currentTilemap.SetColor(tilePosition, Color.white); 
                currentHighlightedTile = null; 
            }
        }

        private MiningConfig.TileMiningData? GetMiningData(TileBase tile)
        {
            foreach (var data in miningConfig.miningDatas)
            {
                if (data.tile == tile)
                {
                    return data;
                }
            }
            return null;
        }

        private Vector3Int WorldToCell(Vector3 worldPosition)
        {
            foreach (var tilemap in tilemaps)
            {
                Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
                if (tilemap.HasTile(cellPosition))
                {
                    return cellPosition;
                }
            }
            return Vector3Int.zero;
        }
    }
}
