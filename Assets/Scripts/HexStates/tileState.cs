using System;
using System.Collections;
using System.Collections.Generic;
using HexStates;
using UnityEngine;

public class TileState : MonoBehaviour
{
    public enum TileStates
    {
        DefaultTile,
        GreenTile,
        BlueTile,
        RedTile,
        LegalTile,
        StarterTile,
        GreenFusionTile,
        BlueFusionTile,
        RedFusionTile,
        DeadTile,
        DestroyerTile,
    }

    public TileStates currentState;

    public event Action<TileStates> OnStateChanged;

    public void init()
    {
        // Set the default state
        if (currentState!= TileStates.StarterTile)
        {
            var thisTile = GetComponent<HexagonTile>();
            currentState = TileStates.DefaultTile;
            ApplyState(thisTile, currentState);
        }
    }
    public void ApplyState(HexagonTile tile, TileStates state)
    {
        currentState = state;
        OnStateChanged?.Invoke(state);
        tile.TileStateChange(state);
    }
    
    
}