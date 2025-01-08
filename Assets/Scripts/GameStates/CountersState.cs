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
                
                // If the hexagon tile is alive, we want to reduce the lifetime of the tile and clear
                // its first turn if it hasn't done so.
                if (!tile.firstTurnCleared)
                {
                    tile.firstTurnCleared = true;
                }
                tile.lifeTime -= 1;
                tile.GetComponentInChildren<TextMeshPro>().text = tile.lifeTime.ToString();
                
                // This function sets the color hue of the tile's text depending on their lifetime.
                if (tile.lifeTime >= GM.resetLifeTimeColor)
                {
                    UiChanger.ChangeTextColor(tile.GetComponentInChildren<TextMeshPro>(), Color.white);
                    //tile.GetComponentInChildren<TextMeshPro>().color = Color.white;
                }
                else if (tile.lifeTime <= GM.firstLifeTimeThreshold && tile.lifeTime > GM.secondLifeTimeThreshold)
                {
                    UiChanger.ChangeTextColor(tile.GetComponentInChildren<TextMeshPro>(), Color.yellow);
                    //tile.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
                }
                else if (tile.lifeTime <= GM.secondLifeTimeThreshold)
                {
                    UiChanger.ChangeTextColor(tile.GetComponentInChildren<TextMeshPro>(), Color.red);
                    //tile.GetComponentInChildren<TextMeshPro>().color = Color.red;
                }
                
                // When the lifetime of the tile reaches 0, we change it to dead, clear text and set first turn cleared to false.
                if (tile.lifeTime <= 0 && tile.isAlive)
                {
                    tile.firstTurnCleared = false;
                    // if dead, remove the text from the tile and change the tile state to dead.
                    tile.GetComponentInChildren<TextMeshPro>().text = "";
                    tile.TileStateChange(HexagonTile.TileStates.DeadTile);
                }

            }
            else // If the tile is not alive, we want to clear the first turn and remove the text from the tile.
            {
                tile.firstTurnCleared = false;
                tile.GetComponentInChildren<TextMeshPro>().text = "";
            }

        }
        
        // if the future tiles are less than 2, we want to generate the next tiles to be placed on the board.
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