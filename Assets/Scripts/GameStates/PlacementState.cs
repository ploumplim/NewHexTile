using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementState : States
{
    public override void Enter()
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
    
    public override void Tick()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                
                // If the ray hits a hexagon tile, we want to apply the next tile state to it.
                HexagonTile hexTile = hit.collider.GetComponent<HexagonTile>();
                if (hexTile != null)
                {
                    if (hexTile.currentTileState == HexagonTile.TileStates.LegalTile)
                    {
                        // If godmode is enabled, we want to apply the tile state that is selected in the godHUD
                        if (GM.GODMODE)
                        {
                            int activeTileIndex = GM.toggleScript.activeTile;
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
                                
                                default:
                                    Debug.LogError("Invalid active tile index: " + activeTileIndex);
                                    break;
                                
                            } 
                        } 
                        else // If godmode is disabled, we want to apply the next tile state to the hexagon tile
                        {
                            NextTileCreate(hexTile);
                        }
                        
                        
                        GM.ChangeState(GM.GetComponent<UpkeepState>());

                        
                    }
                }
            }
        }
    }
    
   
    
    public void NextTileCreate(HexagonTile hexTile)
    {
        hexTile.currentActiveAsset = hexTile.tileVisuals[GM.nextTile];
        switch (GM.nextTile)
        {
            case 0:
                hexTile.TileStateChange(HexagonTile.TileStates.GreenTile);
                break;
            case 1:
                hexTile.TileStateChange(HexagonTile.TileStates.BlueTile);
                break;
            case 2:
                hexTile.TileStateChange(HexagonTile.TileStates.RedTile);
                break;
            case 3:
                hexTile.TileStateChange(HexagonTile.TileStates.DestroyerTile);
                break;
        }
    }
    
}
