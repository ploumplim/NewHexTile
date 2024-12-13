using UnityEngine;

namespace HexStates
{
    public class Tile_Fast : HexagonTile
    {
        public void Init()
        {
            HexagonTile thisTile = GetComponent<HexagonTile>();
            int lifeTime = thisTile.redLifeTime;
            if (thisTile != null)
            {
                thisTile.lifeTime = lifeTime;
                thisTile.isAlive = true;
            }
            LegalizeTiles();
           
        }
    }
}