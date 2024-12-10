using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusion_fast : HexagonTile
{
    public void Init()
    {
        HexagonTile thisTile = GetComponent<HexagonTile>();
        int lifeTime = thisTile.FastLifeTime*2;
        if (thisTile != null)
        {
            thisTile.lifeTime = lifeTime;
            thisTile.isAlive = true;
        }

        LegalizeTiles();

    }
}
