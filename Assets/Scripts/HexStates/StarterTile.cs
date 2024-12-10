// StarterTile.cs

using System;
using UnityEngine;
namespace HexStates
{
    public class StarterTile : HexagonTile
    {
        public new int lifeTime = 1;
        public void Init()
        {
            HexagonTile thisTile = GetComponent<HexagonTile>();
            if (thisTile != null)
            {
                thisTile.lifeTime = lifeTime;
                thisTile.isAlive = true;
            }
            LegalizeTiles();
            
        }
    }
}