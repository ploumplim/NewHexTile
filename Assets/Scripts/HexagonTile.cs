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

    private void Start()
    {
        // Ensure tileState is not null
        tileState = GetComponent<TileState>();
        if (tileState != null)
        {
            tileState.OnStateChanged += HandleStateChanged;
            tileState.ApplyState(this, TileState.TileStates.DefaultState);
        }
    }

    private void HandleStateChanged(TileState.TileStates newState)
    {
        ActivateStateScript(newState);
    }

    public void ActivateStateScript(TileState.TileStates state)
    {
        switch (state)
        {
            case TileState.TileStates.Fusion1:
                Fusion1 fusion1Script = GetComponent<Fusion1>();
                if (fusion1Script == null)
                {
                    fusion1Script = gameObject.AddComponent<Fusion1>();
                }
                break;
            case TileState.TileStates.StarterTile:
                StarterTile starterTileScript = GetComponent<StarterTile>();
                if (starterTileScript == null)
                {
                    starterTileScript = gameObject.AddComponent<StarterTile>();
                }
                break;
        }
    }

    public void ModifyBehavior(TileState.TileStates state)
    {
        // Implement behavior modification based on the state
        switch (state)
        {
            case TileState.TileStates.DefaultState:
                break;
            case TileState.TileStates.TileX:
                GetComponentInChildren<Renderer>().material.color = Color.white;
                break;
            case TileState.TileStates.StarterTile:
                GetComponentInChildren<Renderer>().material.color = Color.red;
                break;
            case TileState.TileStates.Fusion1:
                GetComponentInChildren<Renderer>().material.color = Color.green;
                break;
            default:
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
                    hexTile.isAlive = false;
                    hexTile.tileState.ApplyState(hexTile, TileState.TileStates.DefaultState);
                    hexTile.GetComponentInChildren<Renderer>().material.color = Color.grey;
                    DeleteTileX();
                }
            }
            
        }
    }
    public void ApplyLifeTime(HexagonTile tile)
    {
        switch (tile.tileState.currentState)
        {
            case TileState.TileStates.StarterTile:
                tile.GetComponent<StarterTile>()?.SetLifeTime();
                break;
            case TileState.TileStates.Fusion1:
                tile.GetComponent<Fusion1>()?.SetLifeTime();
                break;
            // Add cases for other states as needed
            default:
                tile.lifeTime = 0; // Default value for unknown states
                break;
        }
    }
    public void TileXCreation()
    {
        HexagonTile hexTile = GetComponent<HexagonTile>();
        if (hexTile != null)
        {
            HexagonTile[] adjacentTiles = hexTile.GetAdjacentTiles();
            foreach (HexagonTile adjacentTile in adjacentTiles)
            {
                if (adjacentTile.tileState.currentState == TileState.TileStates.DefaultState)
                {
                    adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.TileX);
                }
            }
        }
    }
    public void DeleteTileX()
    {
        HexagonTile hexTile = GetComponent<HexagonTile>();
        if (hexTile != null)
        {
            HexagonTile[] adjacentTiles = hexTile.GetAdjacentTiles();
            Debug.Log(adjacentTiles);
            foreach (HexagonTile adjacentTile in adjacentTiles)
            {
                if (adjacentTile.tileState.currentState == TileState.TileStates.TileX)
                {
                    adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.DefaultState);
                }
            }
        }
    }
}