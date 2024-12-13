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
        
        
        // update the living and legal tile lists
        GM.livingTiles = UpdateLivingTileList(GM.Tiles);
        GM.legalTiles = UpdateLegalTileList(GM.Tiles);
        LegalTilesShouldBeDefault(GM.legalTiles);
        
        foreach (HexagonTile tile in GM.livingTiles)
        {
            tile.LegalizeTiles();
        }
        
        GM.ChangeState(GM.GetComponent<CountersState>());
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Effect State");
    }
}
