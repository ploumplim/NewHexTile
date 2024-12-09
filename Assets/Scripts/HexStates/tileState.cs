using System;
using System.Collections;
using System.Collections.Generic;
using HexStates;
using UnityEngine;

public class TileState : MonoBehaviour
{
    public GameObject gridParent;
    public enum TileStates
    {
        DefaultState,
        Fusion1,
        TileX,
        StarterTile
    }

    public TileStates currentState;

    public event Action<TileStates> OnStateChanged;

    private void Start()
    {
        // Set the default state
        currentState = TileStates.DefaultState;
    }

    public void ApplyState(HexagonTile tile, TileStates state)
    {
        currentState = state;
        OnStateChanged?.Invoke(state);
        tile.ModifyBehavior(state);
    }
    
    
}