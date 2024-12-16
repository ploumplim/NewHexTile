using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
}

