// Fusion1.cs
using UnityEngine;

namespace HexStates
{
    public class Fusion1 : HexagonTile
    {
        public void Start()
        {
            isAlive = true;
            TileXCreation();
        }
        
        public void SetLifeTime()
        {
            lifeTime = 5; // Example value for Fusion1
            HexagonTile parentTile = GetComponent<HexagonTile>();
            if (parentTile != null)
            {
                parentTile.lifeTime = lifeTime;
                parentTile.isAlive = true;
            }
        }
    }
}