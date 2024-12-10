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
                        tileState.ApplyState(hexTile, TileState.TileStates.BasicState);
                        GM.changeState(GM.GetComponent<FusionState>());
                        // Apply the lifetime to the tile
                        // hexTile.DecrementLifeTimeForAllTiles(GM.HexGrid);
                    }
                }
            }
        }
    }
    public override void Exit()
    {
        Debug.Log("Exiting Placement State");
    }
}
