using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountersState : States
{
    public override void Enter()
    {
        foreach (Transform child in GM.HexGrid.transform)
        {
            HexagonTile hexTile = child.GetComponent<HexagonTile>();

            if (hexTile.isAlive)
            {
                hexTile.lifeTime -= 1;
                hexTile.GetComponentInChildren<TextMeshPro>().text = hexTile.lifeTime.ToString();
                if (hexTile.lifeTime <= 0)
                {
                    hexTile.GetComponentInChildren<TextMeshPro>().text = "";
                    hexTile.DeadCells();
                }
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