using System.Collections.Generic;
using UnityEngine;

public class UpkeepState : States
{
    public override void Enter()
    {
        //Debug.LogError("Entrer dans UpkeepState");
        // Reset the living tiles list
        GM.livingTiles = new List<HexagonTile>();

        // Add all living tiles to the living tiles list
        // for (int y = 0; y < GM.gridWidth; y++)
        // {
        //     for (int x = 0; x < GM.gridHeight; x++)
        //     {
        //         HexagonTile tile = GM.Tiles[x, y].GetComponentInChildren<HexagonTile>();
        //         
        //         if (tile != null && tile.isAlive)
        //         {
        //             GM.livingTiles.Add(tile);
        //             Debug.LogWarning("Tile added to living tiles: " + tile.transform.position);
        //         }
        //     }
        // }

        foreach (var VARIABLE in GM.livingTiles)
        {
            //Debug.Log(VARIABLE.transform.position);
        }
        // Generate the next tile
        GenerateNextTile();

        // Change the state to placement state
        GM.changeState(GM.GetComponent<PlacementState>());
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {
        //Debug.Log("Current living tiles: " + GM.livingTiles.Count);
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