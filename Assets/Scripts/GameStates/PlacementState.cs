using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : States
{
    public override void Enter()
    {

    }
    
    public override void Tick()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                HexagonTile hexTile = hit.collider.GetComponent<HexagonTile>();
                if (hexTile != null)
                {
                    TileState tileState = hexTile.GetComponent<TileState>();
                    if (tileState != null && tileState.currentState == TileState.TileStates.LegalState)
                    {
                        if (GM.GODMODE)
                        {
                            int activeTileIndex = GM.toggleScript.activeTile;
                            switch (activeTileIndex)
                            {
                                case 0:
                                    tileState.ApplyState(hexTile, TileState.TileStates.BasicState);
                                    break;
                                case 1:
                                    tileState.ApplyState(hexTile, TileState.TileStates.SlowState);
                                    break;
                                case 2:
                                    tileState.ApplyState(hexTile, TileState.TileStates.FastState);
                                    break;
                                case 3:
                                    randomTile(tileState, hexTile);
                                    break;
                                default:
                                    Debug.LogError("Invalid active tile index: " + activeTileIndex);
                                    break;
                            }
                        }
                        else
                        {
                            nextTileCreate();
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
    
    public void randomTile(TileState tileState, HexagonTile hexTile)
    {
        int randomTile = Random.Range(0, 3);
        switch (randomTile)
        {
            case 0:
                
                tileState.ApplyState(hexTile, TileState.TileStates.BasicState);
                break;
            case 1:
                tileState.ApplyState(hexTile, TileState.TileStates.SlowState);
                break;
            case 2:
                tileState.ApplyState(hexTile, TileState.TileStates.FastState);
                break;
            default:
                break;
        }
    }
    
    public void nextTileCreate()
    {
        
    }
}
