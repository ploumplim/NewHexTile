using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexStates
{
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
}