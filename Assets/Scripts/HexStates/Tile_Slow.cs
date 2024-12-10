
using System;
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
        
    }
}