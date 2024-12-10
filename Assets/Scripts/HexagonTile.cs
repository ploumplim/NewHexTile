using System;
using System.Collections.Generic;
using HexStates;
using UnityEditor.SceneManagement;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    [HideInInspector]
    public HexagonGrid parentGrid;
    
    [HideInInspector]
    public TileState tileState;
    
    public bool isAlive;
    public int lifeTime;
    
    
    public int StarterLifeTime = 1;
    
    public int BasicLifeTime = 5;
    public int FastLifeTime = 3;
    public int SlowLifeTime = 7;
    
    public List<TileState.TileStates> stateToFuseWith;
    
    public void InitializeTile(int basicLife, int fastLife, int slowLife)
    {
        BasicLifeTime = basicLife;
        FastLifeTime = fastLife;
        SlowLifeTime = slowLife;
        tileState = GetComponent<TileState>();
        tileState.init();
    }
    
    public void ModifyBehavior(TileState.TileStates state)
    {
        // Implement behavior modification based on the state
        switch (state)
        {
            case TileState.TileStates.DefaultState:
                GetComponentInChildren<Renderer>().material.color = Color.grey;
                Empty defaultTile = GetComponent<Empty>();
                if (defaultTile == null)
                {
                    defaultTile = gameObject.AddComponent<Empty>();
                }
                defaultTile.Init();
                break;
            
            case TileState.TileStates.LegalState:
                GetComponentInChildren<Renderer>().material.color = Color.white;
                Legal legalTile = GetComponent<Legal>();
                if (legalTile == null)
                {
                    legalTile = gameObject.AddComponent<Legal>();
                }
                legalTile.Init();
                break;
            
            case TileState.TileStates.StarterTile:
                GetComponentInChildren<Renderer>().material.color = Color.red;
                StarterTile starterTile = GetComponent<StarterTile>();
                if (starterTile == null)
                {
                    starterTile = gameObject.AddComponent<StarterTile>();
                }
                starterTile.Init();
                break;
            
            case TileState.TileStates.BasicState:
                GetComponentInChildren<Renderer>().material.color = Color.blue;
                Tile_basic basicTile = GetComponent<Tile_basic>();
                if (basicTile == null)
                {
                    basicTile = gameObject.AddComponent<Tile_basic>();
                }
                basicTile.Init();
                
                break;
            case TileState.TileStates.SlowState:
                GetComponentInChildren<Renderer>().material.color = Color.magenta;
                Tile_Slow slowTile = GetComponent<Tile_Slow>();
                if (slowTile == null)
                {
                    slowTile = gameObject.AddComponent<Tile_Slow>();
                }
                slowTile.Init();
                
                break;
            case TileState.TileStates.FastState:
                GetComponentInChildren<Renderer>().material.color = Color.yellow;
                Tile_Fast fastTile = GetComponent<Tile_Fast>();
                if (fastTile == null)
                {
                    fastTile = gameObject.AddComponent<Tile_Fast>();
                }
                fastTile.Init();
                
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

    public void DeadCells()
    {
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.tileState.currentState != TileState.TileStates.LegalState &&
                adjacentTile.tileState.currentState != TileState.TileStates.DefaultState)
            {
                tileState.ApplyState(this, TileState.TileStates.LegalState);
                break;
            }
            else
            {
                tileState.ApplyState(this, TileState.TileStates.DefaultState);
            }
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

    public void CheckToFuseWith(HexagonTile tile,List<TileState.TileStates> stateToFuseWith)
    {
        HexagonTile[] adjacentTiles = tile.GetAdjacentTiles();
        foreach (var adjacentTile in adjacentTiles)
        {
            if (adjacentTile != null && adjacentTile.tileState != null && stateToFuseWith.Contains(adjacentTile.tileState.currentState))
            {
               adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.FastState);
               tile.tileState.ApplyState(tile, TileState.TileStates.FastState);
                break;
            }
        }
    }
}