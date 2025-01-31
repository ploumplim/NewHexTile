using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountersState : States
{
    public override void Enter()
{
    foreach (HexagonTile tile in GM.Tiles)
    {
        if (tile.isAlive)
        {
            if (!tile.firstTurnCleared)
            {
                tile.firstTurnCleared = true;
            }
            tile.lifeTime -= 1;
            tile.Longevity += 1;
            tile.GetComponentInChildren<TextMeshPro>().text = tile.lifeTime.ToString();

            if (tile.lifeTime >= GM.resetLifeTimeColor)
            {
                UiChanger.ChangeTextColor(tile.GetComponentInChildren<TextMeshPro>(), Color.white);
            }
            else if (tile.lifeTime <= GM.firstLifeTimeThreshold && tile.lifeTime > GM.secondLifeTimeThreshold)
            {
                UiChanger.ChangeTextColor(tile.GetComponentInChildren<TextMeshPro>(), Color.yellow);
            }
            else if (tile.lifeTime <= GM.secondLifeTimeThreshold)
            {
                UiChanger.ChangeTextColor(tile.GetComponentInChildren<TextMeshPro>(), Color.red);
            }

            if (tile.lifeTime <= 0 && tile.isAlive)
            {
                tile.firstTurnCleared = false;
                tile.GetComponentInChildren<TextMeshPro>().text = "";
                tile.TileStateChange(HexagonTile.TileStates.DeadTile);
            }

            if (tile.currentTileState == HexagonTile.TileStates.SpreadingTile)
            {
                bool allAdjacentSpreading = true;
                foreach (var adjacentTile in tile.GetAdjacentTiles())
                {
                    if (adjacentTile.currentTileState != HexagonTile.TileStates.SpreadingTile)
                    {
                        allAdjacentSpreading = false;
                        break;
                    }
                }

                if (allAdjacentSpreading)
                {
                    tile.GetComponentInChildren<TextMeshPro>().SetText("");
                }
            }
            
        }
        else
        {
            tile.firstTurnCleared = false;
            tile.GetComponentInChildren<TextMeshPro>().text = "";
        }
    }

    if (GM.futureTileStateList.Count < 2)
    {
        GM.RegenerateFutureTileStateList(GM.futureTilesListCount);
    }

    GM.ChangeState(GM.GetComponent<PlacementState>());
}

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("Legal Tiles : " + GM.legalTiles.Count + ". Living Tiles : " + GM.livingTiles.Count);
    }
}