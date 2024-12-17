using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    [HideInInspector]
    public int greenLifeTime;
    [HideInInspector]
    public int blueLifeTime;
    [HideInInspector]
    public int redLifeTime;
    [HideInInspector]
    public int redFusionLifeTime;
    [HideInInspector]
    public int blueFusionLifeTime;
    [HideInInspector]
    public int greenFusionLifeTime;
    
    
    // This is a list of all the living tiles in the grid.
    [HideInInspector]
    public List<HexagonTile> livingTiles;
    
    //this is a list of all tiles that are currently legal.
    [HideInInspector]
    public List<HexagonTile> legalTiles;
    
    // This is the script that manages godmode.
    [FormerlySerializedAs("toggleScript")] [HideInInspector]
    public GodModeToggleScript godModeToggleScript;
    [HideInInspector]
    public NextTileSelectorToggle nextTileSelectorToggle;
    
    
    //THIS SECTION HAS ALL THE VARIABLES THAT CAN BE CHANGED IN THE INSPECTOR.
    [Header("Initialized Variables")]
    // This is the hexagon grid that we will be using to store the tiles.
        public HexagonGrid hexGrid;
        // This is the toggle that enables godmode.
        public Toggle godModeToggle;
        // This is the HUD that will be displayed when godmode is enabled.
            public GameObject godHUD;
        [FormerlySerializedAs("nextTilePreview")]
        [Tooltip("This determines the next tile that will be placed. " +
                 "At the start of the game, it will generate the number" +
                 "introduced here. (0 = green tile... etc")]
            public GameObject nextTilePreview1;
            public GameObject nextTilePreview2;
            
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
    [FormerlySerializedAs("nextTile")] [Tooltip("Left next tile's ID.")]
    public int nextTile1;
    [Tooltip("Right next tile ID.")]
    public int nextTile2;
    [Tooltip("This is the text that will be displayed when a destroyer tile is previewed.")]
    public string destroyerText = "La Bomba";
    [Tooltip("This is the minimum amount of tiles that need to be placed before we spawn bombs. (Not implemented)")]
    public int destroyerDangerLimit = 4;
    [Header("Tile Weights")]
    public int greenTileWeight = 25;
    public int blueTileWeight = 25;
    public int redTileWeight = 25;
    public int destroyerTileWeight = 25;

    [HideInInspector] public List<int> weights;
    // This is the minimum amount of tiles that need to be placed before we spawn bombs
    
    [Header("Starter Tiles")]
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
        
        // Obtain the lifetime of the different tilestates for output.
        greenLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().greenLifeTime - 1;
        redLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().redLifeTime - 1;
        blueLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().blueLifeTime - 1;
        redFusionLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().redFusionLifeTime - 1;
        blueFusionLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().blueFusionLifeTime - 1;
        greenFusionLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().greenFusionLifeTime - 1;
        
        
        
        // SET STARTER TILE
        var starterTile = hexGrid.TileInstances[starterTileXPosition, starterTileYPosition].GetComponent<HexagonTile>();
          starterTile.TileStateChange(HexagonTile.TileStates.StarterTile);
          
        // Set the current state to the placement state.
        currentState = GetComponent<PlacementState>();
        currentState.Enter();
        
        //Debug.Log(starterTile.gameObject.name);
      
        weights = new List<int> {greenTileWeight, blueTileWeight, redTileWeight, destroyerTileWeight};        


        godModeToggleScript = GetComponent<GodModeToggleScript>();
        nextTileSelectorToggle = GetComponent<NextTileSelectorToggle>();
        
        

    }
    
    private void Update()
    {
        currentState?.Tick();
        if (godModeToggle.isOn)
        {
            godHUD.SetActive(true);
            GODMODE = true;
        }
        else
        {
            godHUD.SetActive(false);
            GODMODE = false;
        }
    }

    public void ChangeState(States newState)
    {
        currentState.Exit();
        currentState = newState;
        //Debug.Log("State changed to " + newState);
        currentState.Enter();
    }
    
    public void RestartScene()
    {
        // Remove current tiles
        foreach (HexagonTile tile in Tiles)
        {
            tile.TileStateChange(HexagonTile.TileStates.DefaultTile);
        }

        // Reset the game state
        currentState = GetComponent<CountersState>();
        currentState.Enter();
        
        // Set the starter tile
        var starterTile = hexGrid.TileInstances[starterTileXPosition, starterTileYPosition].GetComponent<HexagonTile>();
        starterTile.TileStateChange(HexagonTile.TileStates.StarterTile);
    }
    
}
