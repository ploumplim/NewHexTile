using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexStates
{
    
public class Tile_basic : HexagonTile
{
    public void Init()
    {
        HexagonTile thisTile = GetComponent<HexagonTile>();
        int lifeTime = thisTile.BasicLifeTime;
        if (thisTile != null)
        {
            thisTile.lifeTime = lifeTime;
            thisTile.isAlive = true;
        }
        LegalizeTiles();
            
    }
}

}