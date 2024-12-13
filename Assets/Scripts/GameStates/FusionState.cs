using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionState : States
{
    public override void Enter()
    {
        List<HexagonTile> fusableTiles = new List<HexagonTile>();
        
        for (int i = 0; i < GM.livingTiles.Count; i++)
        {
            if (GM.livingTiles[i].CanBeFused())
            {
                HexagonTile tile = GM.livingTiles[i].GetComponent<HexagonTile>();
                if (tile != null)
                {
                    if (tile.isAlive)
                    {
                        fusableTiles.Add(tile);
                    }
                }
            }
        }
        
        
        for (int i = 0; i < fusableTiles.Count; i++)
        {
            if (fusableTiles[i] != null)
            {
                HexagonTile tile = fusableTiles[i].GetComponent<HexagonTile>();
                if (tile != null)
                {
                    tile.FuseTiles();
                }
            }
        }
        
        
        
        
        GM.ChangeState(GM.GetComponent<EffectState>());
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Fusion State");
    }
}
