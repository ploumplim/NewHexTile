using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementState : States
{
    public override void Enter()
    {
        GenerateNextTile();
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
                    TileState tileState = hexTile.GetComponent<TileState>();
                    if (tileState != null && tileState.currentState == TileState.TileStates.LegalTile)
                    {
                        // If godmode is enabled, we want to apply the tile state that is selected in the godHUD
                        if (GM.GODMODE)
                        {
                            int activeTileIndex = GM.toggleScript.activeTile;
                            switch (activeTileIndex)
                            {
                                case 0:
                                    tileState.ApplyState(hexTile, TileState.TileStates.GreenTile);
                                    break;
                                case 1:
                                    tileState.ApplyState(hexTile, TileState.TileStates.BlueTile);
                                    break;
                                case 2:
                                    tileState.ApplyState(hexTile, TileState.TileStates.RedTile);
                                    break;
                                case 3:
                                    tileState.ApplyState(hexTile, TileState.TileStates.DestroyerTile);
                                    break;
                                case 4:
                                    NextTileCreate(tileState, hexTile);
                                    break;
                                default:
                                    Debug.LogError("Invalid active tile index: " + activeTileIndex);
                                    break;
                            }
                        }
                        else // If godmode is disabled, we want to apply the next tile state to the hexagon tile
                        {
                            NextTileCreate(tileState, hexTile);
                        }
                        
                        GM.changeState(GM.GetComponent<UpkeepState>());
                        
                    }
                }
            }
        }
    }
    
    public void GenerateNextTile()
    {
        GM.nextTile = Random.Range(0, 4);
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
    
    public void NextTileCreate(TileState tileState, HexagonTile hexTile)
    {
        switch (GM.nextTile)
        {
            case 0:
                tileState.ApplyState(hexTile, TileState.TileStates.GreenTile);
                break;
            case 1:
                tileState.ApplyState(hexTile, TileState.TileStates.BlueTile);
                break;
            case 2:
                tileState.ApplyState(hexTile, TileState.TileStates.RedTile);
                break;
            case 3:
                tileState.ApplyState(hexTile, TileState.TileStates.DestroyerTile);
                break;
        }
    }
}
