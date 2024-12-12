using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountersState : States
{
    public override void Enter()
    {
        // For each hexagon tile in our livingTiles list, we want to reduce their lifetime.
        foreach (Transform child in GM.hexGrid.transform)
        {
            HexagonTile hexTile = child.GetComponent<HexagonTile>();
            // If the hexagon tile is alive, we want to reduce its lifetime by 1.
            if (hexTile.isAlive)
            {
                hexTile.lifeTime -= 1;
                hexTile.GetComponentInChildren<TextMeshPro>().text = hexTile.lifeTime.ToString();
                if (hexTile.lifeTime <= 0)
                {
                    hexTile.GetComponentInChildren<TextMeshPro>().text = "";
                    hexTile.EndOfLifeTime();
                }
            }
            // If the hexagon tile is not alive, we want to remove the text from the tile.
            else if (!hexTile.isAlive)
            {
                hexTile.GetComponentInChildren<TextMeshPro>().text = "";
            }
        }
        
        GM.changeState(GM.GetComponent<UpkeepState>());
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Counters State");
    }
}