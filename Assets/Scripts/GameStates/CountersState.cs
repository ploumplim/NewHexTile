using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountersState : States
{
    public override void Enter()
    {
        foreach (HexagonTile tile in GM.Tiles)
        {
            // If the hexagon tile is alive, we want to reduce the lifetime of the tile.
            tile.lifeTime -= 1;
            tile.GetComponentInChildren<TextMeshPro>().text = tile.lifeTime.ToString();
            
            if (tile.lifeTime <= 0 && tile.isAlive)
            {
                // if dead, remove the text from the tile and change the tile state to dead.
                tile.GetComponentInChildren<TextMeshPro>().text = "";
                tile.TileStateChange(HexagonTile.TileStates.DeadTile);
            }
            
            // If the hexagon tile is not alive, we want to remove the text from the tile.
            if (!tile.isAlive)
            {
                tile.GetComponentInChildren<TextMeshPro>().text = "";
            }
        }
        
        // Check if the legal tiles should be default and remove it from the legal list.
        LegalTilesShouldBeDefault(GM.legalTiles);
        GM.ChangeState(GM.GetComponent<PlacementState>());
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        // clear all my lists
        GM.legalTiles.Clear();
        GM.livingTiles.Clear();
    }
}