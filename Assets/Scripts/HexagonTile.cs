using System;
using System.Collections.Generic;
using HexStates;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    [HideInInspector]
    public HexagonGrid parentGrid;
    
    [HideInInspector]
    public TileState tileState;
    
    public bool isAlive;
    public int lifeTime;
    

    public void InitializeTile()
    {
        tileState = GetComponent<TileState>();
        tileState.init();
        if (tileState != null)
        {
            tileState.OnStateChanged += ModifyBehavior;
        }
        
        
        
    }

    public void ModifyBehavior(TileState.TileStates state)
    {
        // Implement behavior modification based on the state
        switch (state)
        {
            case TileState.TileStates.DefaultState:
                GetComponentInChildren<Renderer>().material.color = Color.grey;
                
                break;
            
            case TileState.TileStates.LegalState:
                GetComponentInChildren<Renderer>().material.color = Color.white;
                
                break;
            
            case TileState.TileStates.StarterTile:
                GetComponentInChildren<Renderer>().material.color = Color.red;
                StarterTile starterTile = GetComponent<StarterTile>();
                if (starterTile != null)
                {
                    starterTile.Init();
                }
                
                break;
            
            case TileState.TileStates.BasicState:
                GetComponentInChildren<Renderer>().material.color = Color.green;
                Tile_basic basicTile = GetComponent<Tile_basic>();
                if (basicTile != null)
                {
                    basicTile.Init();
                }
                break;
            
            default:
                Debug.Log("The tile state is not recognized:" + transform);
                GetComponentInChildren<Renderer>().material.color = Color.black;
                break;
            
        }
    }

    public HexagonTile[] GetAdjacentTiles()
    {
        List<HexagonTile> adjacentTiles = new List<HexagonTile>();
        Vector3[] directions = new Vector3[]
        {
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3), 0, 0),                                // Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3), 0, 0),                               // Left
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, parentGrid.tileScale * 1.5f),  // Up-Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, parentGrid.tileScale * 1.5f), // Up-Left
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, -parentGrid.tileScale * 1.5f), // Down-Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, -parentGrid.tileScale * 1.5f) // Down-Left
        };

        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, parentGrid.tileScale * 2))
            {
                adjacentTiles.Add(hit.collider.gameObject.GetComponent<HexagonTile>());
            }
        }

        return adjacentTiles.ToArray();
    }

    
    public void DecrementLifeTimeForAllTiles(GameObject gridParent)
    {
        foreach (Transform child in gridParent.transform)
        {
            HexagonTile hexTile = child.GetComponent<HexagonTile>();

            if (hexTile.isAlive)
            {
                hexTile.lifeTime -= 1;
                if (hexTile.lifeTime <= 0)
                {
                    DeadCells(hexTile);
                }
            }
        }
    }

    public void LegalizeTiles()
    {
        HexagonTile hexTile = GetComponent<HexagonTile>();
        if (hexTile != null)
        {
            HexagonTile[] adjacentTiles = hexTile.GetAdjacentTiles();
            
            foreach (HexagonTile adjacentTile in adjacentTiles)
            {
                if (adjacentTile.tileState.currentState == TileState.TileStates.DefaultState)
                {
                    adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.LegalState);
                }
            }
        }
    }

    public void DeadCells(HexagonTile hexTile)
    {
        HexagonTile[] adjacentTiles = hexTile.GetAdjacentTiles();
        hexTile.isAlive = false;

        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.tileState.currentState != TileState.TileStates.LegalState &&
                adjacentTile.tileState.currentState != TileState.TileStates.DefaultState)
            {
                hexTile.tileState.ApplyState(hexTile, TileState.TileStates.LegalState);
                break;
            }
            // else
            // {
            //     hexTile.tileState.ApplyState(hexTile, TileState.TileStates.DefaultState);
            // }
        }
        
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.tileState.currentState == TileState.TileStates.LegalState && CellShouldBeDefault(adjacentTile))
            {
                adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.DefaultState);
            }
        }
    }
    
    public bool CellShouldBeDefault(HexagonTile hexTile) // This method should only be called by dying cells to have any
                                                         // legal tiles around them check if they should still be legal.
    {
        HexagonTile[] adjacentTiles = hexTile.GetAdjacentTiles();
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.tileState.currentState != TileState.TileStates.LegalState &&
                adjacentTile.tileState.currentState != TileState.TileStates.DefaultState)
            {
                return false;
            }
        }
        return true;
    }
    
}