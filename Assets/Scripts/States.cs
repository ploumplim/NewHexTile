using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class States : MonoBehaviour
{
   protected GameManager GM;
   
   public virtual void Initialize(GameManager GM)
   {
      this.GM = GM;
   }

   public virtual void Enter()
   {
      
   }

   public virtual void Tick()
   {
      
   }

   public virtual void Exit()
   {
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
      
   }
   
   public List<HexagonTile> UpdateLegalTileList(HexagonTile[,] tiles) //This list is used to update the legal tiles list.
   {
      List<HexagonTile> newLegalTiles = new List<HexagonTile>();
      
      foreach (HexagonTile tile in tiles)
      { //If the tile is a legal tile, add it to the legal tiles list.
         if (tile.currentTileState == HexagonTile.TileStates.LegalTile)
         {
            newLegalTiles.Add(tile);
         }
         else //If the tile is not a legal tile, remove it from the legal tiles list.
         {
            newLegalTiles.Remove(tile);
         }
      }
      return newLegalTiles; //Return the updated list.
   }
   
   public List<HexagonTile> UpdateLivingTileList(HexagonTile[,] tiles)
   {
      List<HexagonTile> newLivingTiles = new List<HexagonTile>();
      foreach (HexagonTile tile in tiles)
      { //If the tile is alive, add it to the living tiles list.
         if (tile.isAlive)
         {
            newLivingTiles.Add(tile);
         }
         else //If the tile is not alive, remove it from the living tiles list.
         {
            newLivingTiles.Remove(tile);
         }
      }
      
      return newLivingTiles; //Return the updated list.
   }
   
   public void LegalTilesShouldBeDefault(List<HexagonTile> legalTiles)
   {
      foreach (HexagonTile tile in legalTiles.ToList())
      {
         // If the tile has no living adjacent tiles and is a legal tile, change the tile state to default.
         if (!tile.HasLivingAdjacentTiles() &&
             tile.currentTileState == HexagonTile.TileStates.LegalTile)
         {
            tile.TileStateChange(HexagonTile.TileStates.DefaultTile);
         }
      }
   }
   
   public int NextTileGenerator()
    {
        // Generate the next tile to be placed on the board.
        int totalWeight = 0;
        // Calculate the total weight of the weights list with no danger.
        if (GM.livingTiles.Count >= GM.destroyerDangerLimit)
        {
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
                    return i;
              }

           }
        }
        else
        {
           foreach (int weight in GM.weights.Take(GM.weights.Count - 2))
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
                 return i;
              }

           }
        }
        return 0;
    }
}

