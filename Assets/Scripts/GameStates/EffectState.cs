using System.Linq;
using UnityEngine;

public class EffectState : States
{
    public override void Enter()
    {
        // Sort the living tiles based on their state
        // ORDER: rest, Destroyer, Pakku
        var sortedTiles = GM.livingTiles
            .OrderBy(tile => tile.currentTileState == HexagonTile.TileStates.DestroyerTile ? 0 :
                tile.currentTileState == HexagonTile.TileStates.PakkuTile ? 1 :
                tile.currentTileState == HexagonTile.TileStates.GreenTile ? 2 :
                tile.currentTileState == HexagonTile.TileStates.RedTile ? 3 :
                tile.currentTileState == HexagonTile.TileStates.BlueTile ? 4 : 5)
            .ToList();
        
        // foreach (var tile in sortedTiles)
        // {
        //     Debug.Log("Tile State: " + tile.currentTileState);
        // }
        //
        // Debug.Log("--------------------");
        //
        // activate all effects on tiles in the sorted order.
        foreach (HexagonTile tile in sortedTiles)
        {
            if (tile.isAlive)
            {
                tile.GetComponent<HexagonTile>().ActivateTileEffects();
            }
        }
        
        GM.ChangeState(GM.GetComponent<CountersState>());
    }

    public override void Exit()
    {
        base.Exit();
    }
}
