using System.Linq;
using UnityEngine;

public class EffectState : States
{
    public override void Enter()
    {
        // Sort the living tiles based on their state
        // ORDER: rest, Destroyer, Pakku
        var sortedTiles = GM.livingTiles
            .OrderBy(tile => tile.currentTileState == HexagonTile.TileStates.DestroyerTile ? 1 : 0)
            .ThenBy(tile => tile.currentTileState == HexagonTile.TileStates.PakkuTile ? 2 : 0)
            .ToList();
        
        // activate all effects on tiles in the sorted order.
        foreach (HexagonTile tile in sortedTiles)
        {
            
            tile.GetComponent<HexagonTile>().ActivateTileEffects();
        }
        
        GM.ChangeState(GM.GetComponent<CountersState>());
    }

    public override void Exit()
    {
        base.Exit();
    }
}
