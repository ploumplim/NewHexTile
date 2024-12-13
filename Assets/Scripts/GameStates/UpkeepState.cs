using System.Collections.Generic;
using UnityEngine;

public class UpkeepState : States
{
    public override void Enter()
    {
        
        // update the living tiles list
        GM.livingTiles = UpdateLivingTileList(GM.Tiles);
        
        Debug.Log("Current living tiles: " + GM.livingTiles.Count);
        
        foreach (HexagonTile tile in GM.livingTiles)
        {
            tile.LegalizeTiles();
        }
        
        // Update the legal tiles list
        GM.legalTiles = UpdateLegalTileList(GM.Tiles);
        
        
        
        GM.ChangeState(GM.GetComponent<FusionState>());
    }
    
    public override void Tick()
    {
        
    }
    
    public override void Exit()
    {
        //Debug.Log("Current living tiles: " + GM.livingTiles.Length);
    }
    
    
}
