using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionState : States
{
    public override void Enter()
    {
        // reset the list of fusable tiles
        List<HexagonTile> fusableTiles = new List<HexagonTile>();
        
        // add all fusable tiles to my list
        for (int i = 0; i < GM.livingTiles.Count; i++)
        {
            if (GM.livingTiles[i].CanBeFused())
            {
                        fusableTiles.Add(GM.livingTiles[i]);
            }
        }
        
        // fuse the tiles in my list
        for (int i = 0; i < fusableTiles.Count; i++)
        {
            if (fusableTiles[i])
            {
                fusableTiles[i].FuseTiles();
            }
        }
        
        
        
        
        GM.ChangeState(GM.GetComponent<EffectState>());
    }

    public override void Exit()
    {
        // Update my lists
        GM.legalTiles = UpdateLegalTileList(GM.Tiles);
        GM.livingTiles = UpdateLivingTileList(GM.Tiles);
        // Legalize my tiles
        foreach (HexagonTile tile in GM.livingTiles)
        {
            tile.LegalizeTiles();
        }
    }
}
