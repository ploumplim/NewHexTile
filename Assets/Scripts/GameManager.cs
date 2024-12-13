using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector]
    public States currentState;
    // This is the hexagon grid that we will be using to store the tiles.
    public GameObject[,] Tiles;
    // These are the width and height of the grid.
    [HideInInspector]
    public int gridWidth;
    [HideInInspector]
    public int gridHeight;
    
    // This is a list of all the living tiles in the grid.
    [HideInInspector]
    public List<HexagonTile> livingTiles;
    
    // This is the script that manages godmode.
    [HideInInspector]
    public ToggleScript toggleScript;
    
    // This is the next tile that will be placed, generated at upkeep state.
    [HideInInspector] 
    public int nextTile;
    
    
    
    // This is the hexagon grid that we will be using to store the tiles.
    public HexagonGrid hexGrid;

    // This is the HUD that will be displayed when godmode is enabled.
    public GameObject godHUD;
    
    // This is the godmode toggle.
    public bool GODMODE;
    
    // 
    public int starterTileXPosition = 1;
    public int starterTileYPosition = 1;

    [Tooltip("next tile Prefab")]
    public GameObject nextTilePreview;
    
    

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        States[] states = GetComponents<States>();
        foreach (States state in states)
        {
            state.Initialize(this);
        }
        DontDestroyOnLoad(gameObject);
       
        currentState = GetComponent<PlacementState>();
        currentState.Enter();
        hexGrid.InitGrid();
        
        // SET STARTER TILE
        var starterTile = hexGrid.TileInstances[starterTileXPosition, starterTileYPosition].GetComponent<HexagonTile>();
        
        //Debug.Log(starterTile.gameObject.name);
        starterTile.TileStateChange(HexagonTile.TileStates.StarterTile);


        toggleScript = GetComponent<ToggleScript>();
        if (!GODMODE)
        {
            godHUD.SetActive(false);
        }
        
        gridWidth = hexGrid.gridWidth;
        gridHeight = hexGrid.gridHeight;
        Tiles = hexGrid.TileInstances;

    }
    
    private void Update()
    {
        currentState?.Tick();
    }

    public void ChangeState(States newState)
    {
        currentState.Exit();
        currentState = newState;
        //Debug.Log("State changed to " + newState);
        currentState.Enter();
    }
}
