using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector]
    public States currentState;
    
    public GameObject[,] Tiles;
    [HideInInspector]
    public int gridWidth;
    [HideInInspector]
    public int gridHeight;
    [HideInInspector]
    public List<HexagonTile> livingTiles;
    [HideInInspector]
    public ToggleScript toggleScript;
    [HideInInspector] 
    public int nextTile;
    
    
    
    // These variables are used to send information to the hexgrid.
    public HexagonGrid HexGrid;

    public GameObject godHUD;
    
    public bool GODMODE;

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
       
        currentState = GetComponent<UpkeepState>();
        currentState.Enter();
        HexGrid.InitGrid();
        
        // SET STARTER TILE
        HexagonTile starterTile = HexGrid.TileInstances[starterTileXPosition, starterTileYPosition].GetComponent<HexagonTile>();
        
        //Debug.Log(starterTile.gameObject.name);
        starterTile.GetComponent<TileState>().ApplyState(starterTile, TileState.TileStates.StarterTile);


        toggleScript = GetComponent<ToggleScript>();
        if (!GODMODE)
        {
            godHUD.SetActive(false);
        }
        
        gridWidth = HexGrid.gridWidth;
        gridHeight = HexGrid.gridHeight;
        Tiles = HexGrid.TileInstances;

    }
    
    private void Update()
    {
        currentState?.Tick();
    }

    public void changeState(States newState)
    {
        currentState.Exit();
        currentState = newState;
        //Debug.Log("State changed to " + newState);
        currentState.Enter();
    }
}
