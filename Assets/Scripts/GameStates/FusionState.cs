using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionState : States
{
    public override void Enter()
    {
        for (int y = 0; y < GM.gridWidth; y++)
        {
            for (int x = 0; x < GM.gridHeight; x++)
            {
                HexagonTile tile = GM.Tiles[x, y].GetComponentInChildren<HexagonTile>();

                if (tile != null && tile.isAlive && !GM.livingTiles.Contains(tile))
                {
                    GM.livingTiles.Add(tile);
                    Debug.Log($"Tile added: {tile.transform.position}");
                }
            }
        }
        Debug.LogWarning("Starting fusion process...");
        //Debug.LogError("Entrer dans FusionState");
        
        //For each hexagon tile in our livingTiles list, we want run their fuse function.
        foreach (var livingTile in GM.livingTiles)
        {
            Debug.Log($"Fusing tile at position: {livingTile.transform.position}");
            livingTile.FuseTiles(livingTile);
        }
        Debug.LogWarning("Fusion process completed.");
        
            //Debug.Log(GM.livingTiles[i].transform.position);
            // if (GM.livingTiles[i] != null)
            // {
            //     HexagonTile tile = GM.livingTiles[i].GetComponent<HexagonTile>();
            //     if (tile != null)
            //     {
            //         //tile.priorityScore=tile.lifeTime;
            //         tile.FuseTiles(tile);
            //     }
            //    
            // }
        
       
        
        GM.changeState(GM.GetComponent<EffectState>());
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        Debug.Log("Exiting Fusion State");
    }
}
