using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementState : States
{

    private HexagonTile previousHexTile;
    public override void Enter()
    {
        // Generate the next tile to be placed on the board by updating the next tile state to the first element in the future tile list.
        
        // Generate a preview of the next two tiles to be placed on the board.
        GM.CorrectTileUIPreviews(0);
        GM.CorrectTileUIPreviews(1);
    }
    
    public override void Tick()
    {
        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // If the ray hits a hexagon tile, we want to apply the next tile state to it.
                HexagonTile hexTile = hit.collider.GetComponent<HexagonTile>();
                
                
               
                
                if (hexTile != null)
                {
                    if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
                    {    
                        if (hexTile.currentTileState == HexagonTile.TileStates.LegalTile)
                        {
                            // If godmode is enabled, we want to apply the tile state that is selected in the godHUD
                            if (GM.GODMODE)
                            {
                                int activeTileIndex = GM.godModeToggleScript.activeTile;
                                switch (activeTileIndex)
                                {
                                    case 0:
                                        NextTileCreate(hexTile);
                                        break;
                                    case 1:
                                        hexTile.TileStateChange(HexagonTile.TileStates.GreenTile);
                                        break;
                                    case 2:
                                        hexTile.TileStateChange(HexagonTile.TileStates.BlueTile);
                                        break;
                                    case 3:
                                        hexTile.TileStateChange(HexagonTile.TileStates.RedTile);
                                        break;
                                    case 4:
                                        hexTile.TileStateChange(HexagonTile.TileStates.DestroyerTile);
                                        break;
                                    case 5:
                                        hexTile.TileStateChange(HexagonTile.TileStates.PakkuTile);
                                        break;
                                    
                                    default:
                                        Debug.LogError("Invalid active tile index: " + activeTileIndex);
                                        break;
                                    
                                } 
                            } 
                            else // If godmode is disabled, we want to apply the next tile state to the hexagon tile
                            {
                                NextTileCreate(hexTile);
                            }
                        
                        
                            GM.ChangeState(GM.GetComponent<EffectState>());

                        
                        }
                    }
                }
                
                
                if (hexTile.currentTileState == HexagonTile.TileStates.LegalTile)
                {

                    if (hexTile!= previousHexTile)
                    {
                        
                        if (previousHexTile != null)
                        {
                            if (previousHexTile.currentTileState == HexagonTile.TileStates.LegalTile)
                            {
                                foreach (var VARIABLE in previousHexTile.tileVisuals)
                                {
                                    VARIABLE.SetActive(false);   
                                }
                                previousHexTile.tileVisuals[1].SetActive(true); 
                            }
                            
                        }
                        previousHexTile = hexTile;
                    }
                    
                    
                    switch (GM.futureTileStateList[0])
                    {
                        case HexagonTile.TileStates.GreenTile:
                            foreach (var visualsTile in hexTile.tileVisuals)
                            {
                                visualsTile.SetActive(false);
                            }
                            hexTile.tileVisuals[3].SetActive(true);
                            break;
                        case HexagonTile.TileStates.BlueTile:
                            foreach (var visualsTile in hexTile.tileVisuals)
                            {
                                visualsTile.SetActive(false);
                            }
                            hexTile.tileVisuals[4].SetActive(true);
                            
                            break;
                        case HexagonTile.TileStates.RedTile:
                            
                            foreach (var visualsTile in hexTile.tileVisuals)
                            {
                                visualsTile.SetActive(false);
                            }
                            hexTile.tileVisuals[5].SetActive(true);
                            break;
                        case HexagonTile.TileStates.DestroyerTile:
                            foreach (var visualsTile in hexTile.tileVisuals)
                            {
                                visualsTile.SetActive(false);
                            }
                            hexTile.tileVisuals[10].SetActive(true);
                            break;
                        case HexagonTile.TileStates.PakkuTile:
                            foreach (var visualsTile in hexTile.tileVisuals)
                            {
                                visualsTile.SetActive(false);
                            }
                            hexTile.tileVisuals[11].SetActive(true);
                            break;
                    }
                }
                
                
                
                
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GM.SwapIncomingTiles(); 
            }
    }
    
   
    
    public void NextTileCreate(HexagonTile hexTile)
    { 
        
        // Apply the next tile state to the hexagon tile
      hexTile.TileStateChange(GM.futureTileStateList[0]);
      // Remove the first tile from the future tile list.
      GM.futureTileStateList.RemoveAt(0);

    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
