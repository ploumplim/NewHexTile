using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexStates
{
    
public class Tile_basic : HexagonTile
{
    // Start is called before the first frame update
    public void Start()
    {
        isAlive = true;
        LegalizeTiles();
    }
        
    public void Init()
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