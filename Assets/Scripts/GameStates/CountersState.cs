using System.Collections;
using System.Collections.Generic;
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
        
        // Check if the legal tiles should be default and remove it from the legal list.
        LegalTilesShouldBeDefault(GM.legalTiles);
        GenerateNextTile();
        
        
        GM.ChangeState(GM.GetComponent<PlacementState>());
        
        
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        // clear all my lists
        GM.legalTiles.Clear();
        GM.livingTiles.Clear();
    }
    
    public void GenerateNextTile()
    {
        if (GM.livingTiles.Count < GM.destroyerDangerLimit)
        {
            GM.nextTile = Random.Range(0, 3);
        }

        if (GM.livingTiles.Count >= GM.destroyerDangerLimit)
        {
            GM.nextTile = Random.Range(0, 4);
        }

        switch (GM.nextTile)
        {
            case 0: //basic tile
                GM.nextTilePreview.GetComponentInChildren<Image>().color = new Color(0f, 0.5f, 0f);
                GM.nextTilePreview.GetComponentInChildren<TextMeshProUGUI>().text = "Green Tile";
                break;
            case 1: //slow tile
                GM.nextTilePreview.GetComponentInChildren<Image>().color = new Color(0f, 0f, 0.5f);
                GM.nextTilePreview.GetComponentInChildren<TextMeshProUGUI>().text = "Blue Tile";
                break;
            case 2: //fast tile
                GM.nextTilePreview.GetComponentInChildren<Image>().color = new Color(0.5f, 0f, 0f);
                GM.nextTilePreview.GetComponentInChildren<TextMeshProUGUI>().text = "Red Tile";
                break;
            case 3: //destroyer tile
                GM.nextTilePreview.GetComponentInChildren<Image>().color = new Color(1f, 0f, 1f);
                GM.nextTilePreview.GetComponentInChildren<TextMeshProUGUI>().text = "La Bomba";
                break;
        }
    }
}