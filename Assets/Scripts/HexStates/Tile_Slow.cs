using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Slow : HexagonTile
{
    public void Init()
    {
        HexagonTile thisTile = GetComponent<HexagonTile>();
        int lifeTime = thisTile.SlowLifeTime;
        if (thisTile != null)
        {
            thisTile.lifeTime = lifeTime;
            thisTile.isAlive = true;
        }
        LegalizeTiles();
        
        CheckToFuseWith(thisTile, stateToFuseWith);
    }
    public Tile_Slow()
    {
        stateToFuseWith = new List<TileState.TileStates>
        {
            TileState.TileStates.SlowState
        };
    }
}