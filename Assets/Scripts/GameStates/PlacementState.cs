using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : States
{
    public override void Enter()
    {
        //Debug.LogError("EnterToPlacement");
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
                    Debug.Log("Hexagon tile clicked: " + hexTile.transform.position);
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

                        GM.changeState(GM.GetComponent<FusionState>());
                        
                    }
                }
            }
        }
    }
    public override void Exit()
    {
        //Debug.Log("Exiting Placement State");
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
