    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UpkeepState : States
{
    public override void Enter()
    {
        GM.livingTiles = new GameObject[GM.gridWidth * GM.gridHeight];
        int livingTilePosition = 0;
        for (int y = 0; y < GM.gridWidth; y++)
        {
            for (int x = 0; x < GM.gridHeight; x++)
            {
                // float xPosition = y % 2 == 0 ? x * GM.tileScale : x * GM.tileScale + GM.tileScale / 2.0f;
                // float yPosition = y * 0.9f * GM.tileScale;
                // Vector3 position = new Vector3(xPosition, 0.0f, yPosition);
                // Vector3 scale = Vector3.one * GM.tileScale;

                GameObject tile = GM.Tiles[x,y];
                if (tile != null && tile.GetComponent<HexagonTile>().isAlive)
                {
                    GM.livingTiles[livingTilePosition] = tile;
                    livingTilePosition++;
                }
                
                
                
            }
        }
        
        generateNextTile();
        
        GM.changeState(GM.GetComponent<PlacementState>());
    }
    
    public override void Tick()
    {
        
    }
    
    public override void Exit()
    {
        //Debug.Log("Current living tiles: " + GM.livingTiles.Length);
    }
    
    public void generateNextTile()
    {
        GM.nextTile = Random.Range(0, 3);
        HexagonGrid hexGrid = GM.HexGrid;
        switch (GM.nextTile)
        {
            case 0:
                GM.nextTilePreview.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.blue;
                break;
            case 1:
                GM.nextTilePreview.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.magenta;
                break;
            case 2:
                GM.nextTilePreview.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.yellow;
                break;
        }
    }
}
