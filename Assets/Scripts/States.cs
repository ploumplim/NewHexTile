using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class States : MonoBehaviour
{
   public GameManager GM;
   
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
      GM.lawfulTiles = UpdateLawfulTileList(GM.Tiles);
      // Legalize my tiles
      foreach (HexagonTile tile in GM.lawfulTiles)
      {
         tile.LegalizeTiles();
      }
      
   }
   
   private List<HexagonTile> UpdateLivingTileList(HexagonTile[,] tiles)
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
   
   private List<HexagonTile> UpdateLawfulTileList(HexagonTile[,] tiles)
   {
      List<HexagonTile> newLawfulTiles = new List<HexagonTile>();
      foreach (HexagonTile tile in tiles)
      { //If the tile is lawful, add it to the lawful tiles list.
         if (tile.canLegalize)
         {
            newLawfulTiles.Add(tile);
         }
         else //If the tile is not lawful, remove it from the lawful tiles list.
         {
            newLawfulTiles.Remove(tile);
         }
      }
      
      return newLawfulTiles; //Return the updated list.
   }
   
   private void LegalTilesShouldBeDefault(List<HexagonTile> legalTiles)
   {
      foreach (HexagonTile tile in legalTiles.ToList())
      {
         // If the tile has no living adjacent tiles and is a legal tile, change the tile state to default.
         if (!tile.HasLegalizingTiles() &&
             tile.currentTileState == HexagonTile.TileStates.LegalTile)
         {
            tile.TileStateChange(HexagonTile.TileStates.DefaultTile);
         }
      }
   }
   

   

      // foreach (HexagonTile.TileStates tileState in GM.futureTileStateList)
      // {
      //    Debug.Log(tileState);
      // }
   }
   
   
   
   
   


