// StarterTile.cs

using System;
using UnityEngine;
namespace HexStates
{
    public class StarterTile : HexagonTile
    {
        public void Init()
        {
            HexagonTile thisTile = GetComponent<HexagonTile>();
            int lifeTime = thisTile.starterLifeTime;
            if (thisTile != null)
            {
                thisTile.lifeTime = lifeTime;
                thisTile.isAlive = true;
            }
            LegalizeTiles();
        }
    }
}