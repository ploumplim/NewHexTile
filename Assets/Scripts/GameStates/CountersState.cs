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
            // If the hexagon tile is alive, we want to reduce the lifetime of the tile.
            tile.lifeTime -= 1;
            

            tile.GetComponentInChildren<TextMeshPro>().text = tile.lifeTime.ToString();
            // This function sets the color hue of the tile's text depending on their lifetime.
            if (tile.lifeTime >= GM.resetLifeTimeColor)
            {
                tile.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }
            else if (tile.lifeTime <= GM.firstLifeTimeThreshold && tile.lifeTime > GM.secondLifeTimeThreshold)
            {
                tile.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
            }
            else if (tile.lifeTime <= GM.secondLifeTimeThreshold)
            {
                tile.GetComponentInChildren<TextMeshPro>().color = Color.red;
            }
            
            if (tile.lifeTime <= 0 && tile.isAlive)
            {
                // if dead, remove the text from the tile and change the tile state to dead.
                tile.GetComponentInChildren<TextMeshPro>().text = "";
                tile.TileStateChange(HexagonTile.TileStates.DeadTile);
            }
            
            // If the hexagon tile is not alive, we want to remove the text from the tile.
            if (!tile.isAlive)
            {
                tile.GetComponentInChildren<TextMeshPro>().text = "";
            }
            
        }
        // Generate the next tile to be placed on the board.
        GM.nextTile1 = NextTileGenerator();
        do
        {
            GM.nextTile2 = NextTileGenerator();
        } while (GM.nextTile1 == GM.nextTile2);
        
        GM.ChangeState(GM.GetComponent<PlacementState>());

    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("Legal Tiles : " + GM.legalTiles.Count + ". Living Tiles : " + GM.livingTiles.Count);
    }
}