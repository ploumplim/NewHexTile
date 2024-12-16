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
        
        
        // Check if the legal tiles should be default from all my tiles
        LegalTilesShouldBeDefault(GM.Tiles.Cast<HexagonTile>().ToList());
        // Update my living tiles list
        GM.livingTiles = UpdateLivingTileList(GM.Tiles);
        // Legalize my tiles
        foreach (HexagonTile tile in GM.livingTiles)
        {
            tile.LegalizeTiles();
        }
        // Update my legal tiles list
        GM.legalTiles = UpdateLegalTileList(GM.Tiles);
        
        // Generate the next tile to be placed on the board.
        GenerateNextTile();
        
        
        GM.ChangeState(GM.GetComponent<PlacementState>());
        
        
    }

   public void GenerateNextTile()
    {
        // Generate the next tile to be placed on the board.
        int totalWeight = 0;
        // Calculate the total weight of the weights list.
        foreach (int weight in GM.weights)
        {
            totalWeight += weight;
        }
        
        // Generate a random value between 0 and the total weight.
        int randomValue = Random.Range(0, totalWeight);
        
        // Calculate the cumulative weight of the weights list.
        int cumulativeWeight = 0;
        
        // Iterate through the weights list and set the next tile to the index of the weight that the random value is less than.
        for (int i = 0; i < GM.weights.Count; i++)
        {
            cumulativeWeight += GM.weights[i];
            if (randomValue < cumulativeWeight)
            {
                if (GM.livingTiles.Count >= GM.destroyerDangerLimit)
                {
                    GM.nextTile = i;
                }
                else
                {
                    GM.nextTile = Random.Range(0, GM.weights.Count - 1);
                }
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