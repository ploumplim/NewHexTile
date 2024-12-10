// StarterTile.cs

using System;
using UnityEngine;
namespace HexStates
{
    public class Empty : HexagonTile
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