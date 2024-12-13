// HexagonGrid.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGrid : MonoBehaviour
{
    [SerializeField] public int gridWidth = 60;
    [SerializeField] public int gridHeight = 60;
    [SerializeField] public float tileScale = 1f;
    [SerializeField] private GameObject hexagonTilePrefab;

    [SerializeField] public GameObject[,] TileInstances;
    
    private void Awake()
    {
        TileInstances = new GameObject[gridWidth, gridHeight];
    }

    public void InitGrid()
    {
        for (int y = 0; y < gridWidth; y++)
        {
            for (int x = 0; x < gridHeight; x++)
            {
                float xPosition = y % 2 == 0 ? x * tileScale : x * tileScale + tileScale / 2.0f;
                float yPosition = y * 0.9f * tileScale;
                Vector3 position = new Vector3(xPosition, 0.0f, yPosition);
                Vector3 scale = Vector3.one * tileScale;

                TileInstances[x, y] = Instantiate(hexagonTilePrefab, position, Quaternion.identity, transform);
                TileInstances[x, y].transform.localScale = scale; 
                HexagonTile tile = TileInstances[x, y].GetComponent<HexagonTile>();
                if (tile != null)
                {
                    tile.InitializeTile();
                    tile.parentGrid = this;
                }
            }
        }
    }
    
}