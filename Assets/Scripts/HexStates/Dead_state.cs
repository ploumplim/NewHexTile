// StarterTile.cs

using System;
using UnityEngine;
namespace HexStates
{
    public class Dead_state : HexagonTile
    {
        public void Init()
        {
            HexagonTile thisTile = GetComponent<HexagonTile>();
            if (thisTile != null)
            {
                thisTile.isAlive = false;
            }
            
        }
    }
}