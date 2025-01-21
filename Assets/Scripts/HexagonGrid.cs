using System;
using System.Collections.Generic;
using Editor;
using GameStates;
using UnityEngine;

public class HexagonGrid : MonoBehaviour
{
    [SerializeField] public int gridWidth;
    [SerializeField] public int gridHeight;
    [SerializeField] public float tileScale = 1f;
    [SerializeField] private GameObject hexagonTilePrefab;

    [SerializeField] public HexagonTile[,] TileInstances;

    public LevelStarter LevelData;

    public void InitGrid()
    {
        gridHeight = LevelData.selectedLevelExotic.gridX;
        gridWidth = LevelData.selectedLevelExotic.gridY;
        TileInstances = new HexagonTile[gridWidth, gridHeight];
        for (int y = 0; y < gridWidth; y++)
        {
            for (int x = 0; x < gridHeight; x++)
            {
                GameObject[,] tileGameObjects = new GameObject[gridWidth, gridHeight];
                float xPosition = y % 2 == 0 ? x * tileScale : x * tileScale + tileScale / 2.0f;
                float yPosition = y * 0.9f * tileScale;
                Vector3 position = new Vector3(xPosition, 0.0f, yPosition);
                Vector3 scale = Vector3.one * tileScale;

                tileGameObjects[x, y] = Instantiate(hexagonTilePrefab, position, Quaternion.identity, transform);
                tileGameObjects[x, y].transform.localScale = scale; 
                
                TileInstances[x, y] = tileGameObjects[x, y].GetComponent<HexagonTile>();
                
                HexagonTile tile = TileInstances[x, y];
                
                if (tile != null)
                {
                    tile.InitializeTile();
                    tile.parentGrid = this;
                    // Set the tile state based on the Level data
                    TileInfo tileInfo = LevelData.selectedLevelExotic.tiles.Find(t => t.x == x && t.y == y);
                    if (tileInfo != null)
                    {
                        tile.TileStateChange(tileInfo.tileState);
                    }
                }
            }
        }
    }
}