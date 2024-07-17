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
        public Tilemap tilemap;

        private TileBase currentHighlightedTile;
        private Vector3Int previousMousePosition;
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
                // Reset tile color when exiting edit mode
                tilemap.SetColor(previousMousePosition, Color.white);
                currentHighlightedTile = null;
            }
        }

        private void HandleTileHighlight()
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mousePos = tilemap.WorldToCell(worldPos);

            if (mousePos != previousMousePosition)
            {
                if (currentHighlightedTile != null)
                {
                    tilemap.SetColor(previousMousePosition, Color.white);  // Reset highlight
                }

                if (tilemap.HasTile(mousePos))
                {
                    currentHighlightedTile = tilemap.GetTile(mousePos);
                    tilemap.SetColor(mousePos, Color.yellow);  // Highlight the tile
                    previousMousePosition = mousePos;
                }
                else
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
            if (tilemap.GetTile(position) == currentHighlightedTile)
            {
                float progress = (Time.time - startTime) / GetMiningData(currentHighlightedTile).Value.miningTime;
                tilemap.SetColor(position, Color.Lerp(Color.yellow, Color.black, progress));

                if (progress >= 1.0f)
                {
                    BreakTile(position);
                }
            }
        }

        private void BreakTile(Vector3Int tilePosition)
        {
            var miningData = GetMiningData(tilemap.GetTile(tilePosition));
            if (miningData != null)
            {
                tilemap.SetTile(tilePosition, null);  // Remove the tile
                Instantiate(miningData.Value.itemConfig.itemPrefab, tilemap.GetCellCenterWorld(tilePosition), Quaternion.identity);  // Drop item
                tilemap.SetColor(tilePosition, Color.white);  // Reset color
                currentHighlightedTile = null;  // Clear the highlighted tile
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
    }
}
