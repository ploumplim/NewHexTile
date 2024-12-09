// StarterTile.cs
using UnityEngine;

public class StarterTile : HexagonTile
{
    public void Start()
    {
        isAlive = true;
        TileXCreation();
    }

    public void SetLifeTime()
    {
        lifeTime = 40; // Example value for StarterTile
        HexagonTile parentTile = GetComponent<HexagonTile>();
        if (parentTile != null)
        {
            parentTile.lifeTime = lifeTime;
            parentTile.isAlive = true;
        }
        Debug.Log("StarterTile lifetime set to: " + lifeTime);
    }
}