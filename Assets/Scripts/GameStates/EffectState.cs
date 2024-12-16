using System.Linq;
using UnityEngine;

public class EffectState : States
{
    public override void Enter()
    {
        // activate all effects on tiles.
        foreach (HexagonTile tile in GM.livingTiles.ToList())
        {
            tile.GetComponent<HexagonTile>().ActivateTileEffects();
        }
        
        GM.ChangeState(GM.GetComponent<CountersState>());
    }

    public override void Exit()
    {
        // Check if the legal tiles should be default from all my tiles
        LegalTilesShouldBeDefault(GM.Tiles.Cast<HexagonTile>().ToList());
        // Update my living tiles list
        GM.livingTiles = UpdateLivingTileList(GM.Tiles);
        // Legalize my tiles
        foreach (HexagonTile tile in GM.livingTiles)
        {
            tile.LegalizeTiles();
        }
        // Update my legal tiles list
        GM.legalTiles = UpdateLegalTileList(GM.Tiles);
    }
}
