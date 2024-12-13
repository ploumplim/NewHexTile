using UnityEngine;

namespace HexStates
{
    public class Fusion1 : HexagonTile
    {
        public void Init()
        {
            HexagonTile thisTile = GetComponent<HexagonTile>();
            int lifeTime = thisTile.greenFusionLifeTime;
            if (thisTile != null)
            {
                thisTile.lifeTime += lifeTime;
                thisTile.isAlive = true;
            }
            LegalizeTiles();
            
        }
    }
}