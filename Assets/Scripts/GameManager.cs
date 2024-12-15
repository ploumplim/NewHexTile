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
    public HexagonTile[,] Tiles;
    // These are the width and height of the grid.
    [HideInInspector]
    public int gridWidth;
    [HideInInspector]
    public int gridHeight;
    
    // This is a list of all the living tiles in the grid.
    [HideInInspector]
    public List<HexagonTile> livingTiles;
    
    //this is a list of all tiles that are currently legal.
    [HideInInspector]
    public List<HexagonTile> legalTiles;
    
    // This is the script that manages godmode.
    [HideInInspector]
    public ToggleScript toggleScript;
    
    
    //THIS SECTION HAS ALL THE VARIABLES THAT CAN BE CHANGED IN THE INSPECTOR.
    [Header("Initialized Variables")]
    // This is the hexagon grid that we will be using to store the tiles.
        public HexagonGrid hexGrid;
        // This is the HUD that will be displayed when godmode is enabled.
            public GameObject godHUD;
    // This is the godmode toggle.
        [Tooltip("next tile Prefab")]
            public GameObject nextTilePreview;
            
        [Space]
    [Header("LifeTime Thresholds")]
    [Tooltip("This is the minimum amount of lifetime a tile needs to reset the color of the text.")]
    public int resetLifeTimeColor = 3;
    [Tooltip("This is the first threshold for the lifetime of the tile. If the tile reaches this lifetime, the text will turn yellow.")]
    public int firstLifeTimeThreshold = 2;
    [Tooltip("This is the second threshold for the lifetime of the tile. If the tile reaches this lifetime, the text will turn red.")]
    public int secondLifeTimeThreshold = 1;
    
    // ada space in the inspector to separate the terms of a custom size
    [Space]
    
    [Header("Miscellaneous Game Variables")]// This is the next tile that will be placed, randomly generated at the end of each turn.
    [Tooltip("Activate godmode")]
    public bool GODMODE;
    [Tooltip("This is the porcentage rng that defines the next tile.")]
    public int nextTile;
    
    [Header("Tile Weights")]
    public int greenTileWeight = 25;
    public int blueTileWeight = 25;
    public int redTileWeight = 25;
    public int destroyerTileWeight = 25;

    [HideInInspector] public List<int> weights;
    // This is the minimum amount of tiles that need to be placed before we spawn bombs
    
    
    public int destroyerDangerLimit = 4;
    
       // starter tile position
    public int starterTileXPosition = 1;
    public int starterTileYPosition = 1;
    

    

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
        
        // Initialize the grid
        hexGrid.InitGrid();
        
        gridWidth = hexGrid.gridWidth;
        gridHeight = hexGrid.gridHeight;
        Tiles = hexGrid.TileInstances;
        
        // SET STARTER TILE
        var starterTile = hexGrid.TileInstances[starterTileXPosition, starterTileYPosition].GetComponent<HexagonTile>();
          starterTile.TileStateChange(HexagonTile.TileStates.StarterTile);
          
        // Set the current state to the placement state.
        currentState = GetComponent<PlacementState>();
        currentState.Enter();
        
        //Debug.Log(starterTile.gameObject.name);
      
        weights = new List<int> {greenTileWeight, blueTileWeight, redTileWeight, destroyerTileWeight};        


        toggleScript = GetComponent<ToggleScript>();
        if (!GODMODE)
        {
            godHUD.SetActive(false);
        }
        
        

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
