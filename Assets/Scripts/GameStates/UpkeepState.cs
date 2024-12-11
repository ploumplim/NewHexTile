using System.Collections.Generic;
using UnityEngine;

public class UpkeepState : States
{
    public override void Enter()
    {
        GM.livingTiles = new List<HexagonTile>();
        for (int y = 0; y < GM.gridWidth; y++)
        {
            for (int x = 0; x < GM.gridHeight; x++)
            {
                GameObject tile = GM.Tiles[x,y];
                if (tile != null && tile.GetComponentInChildren<HexagonTile>().isAlive)
                {
                    GM.livingTiles.Add(tile.GetComponentInChildren<HexagonTile>());
                }
            }
        }
        
        GenerateNextTile();
        
        GM.changeState(GM.GetComponent<PlacementState>());
    }
    
    public override void Tick()
    {
        
    }
    
    public override void Exit()
    {
        //Debug.Log("Current living tiles: " + GM.livingTiles.Length);
    }
    
    public void GenerateNextTile()
    {
        GM.nextTile = Random.Range(0, 4);
        switch (GM.nextTile)
        {
            case 0: //basic tile
                GM.nextTilePreview.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(0f, 0.5f, 0f);
                break;
            case 1: //slow tile
                GM.nextTilePreview.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(0f, 0f, 0.5f);
                break;
            case 2: //fast tile
                GM.nextTilePreview.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(0.5f, 0f, 0f);
                break;
            case 3: //destroyer tile
                GM.nextTilePreview.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(1f, 0f, 1f);
                break;
        }
    }
}
