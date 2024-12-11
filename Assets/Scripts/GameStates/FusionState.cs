using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionState : States
{
    public override void Enter()
    {
        for (int i = 0; i < GM.livingTiles.Length; i++)
        {
            if (GM.livingTiles[i] != null)
            {
                HexagonTile tile = GM.livingTiles[i].GetComponent<HexagonTile>();
                if (tile != null)
                {
                    tile.FuseTiles(tile);
                }
            }
        }
        
        
        
        
        GM.changeState(GM.GetComponent<EffectState>());
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Fusion State");
    }
}
