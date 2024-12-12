using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionState : States
{
    public override void Enter()
    {
        // For each hexagon tile in our livingTiles list, we want run their fuse function.
        for (int i = 0; i < GM.livingTiles.Count; i++)
        {
            if (GM.livingTiles[i] != null)
            {
                HexagonTile tile = GM.livingTiles[i].GetComponent<HexagonTile>();
                if (tile != null)
                {
                    //tile.priorityScore=tile.lifeTime;
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
