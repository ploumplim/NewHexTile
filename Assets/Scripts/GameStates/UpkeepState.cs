using System.Collections.Generic;
using UnityEngine;

public class UpkeepState : States
{
    public override void Enter()
    {
        
        // Reset the living tiles list
        GM.livingTiles = new List<HexagonTile>();
        
        // Add all living tiles to the living tiles list
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
        
        
        // Change the state to fusion state
        GM.changeState(GM.GetComponent<FusionState>());
    }
    
    public override void Tick()
    {
        
    }
    
    public override void Exit()
    {
        //Debug.Log("Current living tiles: " + GM.livingTiles.Length);
    }
    
    
}
