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
        int totalWeight = 0;
        
        foreach (int weight in GM.weights)
        {
            totalWeight += weight;
        }
        
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        
        for (int i = 0; i < GM.weights.Count; i++)
        {
            cumulativeWeight += GM.weights[i];
            if (randomValue < cumulativeWeight)
            {
                GM.nextTile = i;
                break;
            }
        }
        
        
        switch (GM.nextTile)
        {
            case 0: //green tile
                GM.nextTilePreview.GetComponentInChildren<Image>().color = new Color(0f, 0.5f, 0f);
                GM.nextTilePreview.GetComponentInChildren<TextMeshProUGUI>().text = "Green Tile";
                break;
            case 1: //blue tile
                GM.nextTilePreview.GetComponentInChildren<Image>().color = new Color(0f, 0f, 0.5f);
                GM.nextTilePreview.GetComponentInChildren<TextMeshProUGUI>().text = "Blue Tile";
                break;
            case 2: //red tile
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