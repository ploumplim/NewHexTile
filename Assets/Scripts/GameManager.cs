using System;
using UnityEngine;

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
    public float tileScale;
    [HideInInspector]
    public GameObject[] livingTiles;
    [HideInInspector]
    public ToggleScript toggleScript;
    [HideInInspector] 
    public int nextTile;
    
    
    
    // These variables are used to send information to the hexgrid.
    public HexagonGrid HexGrid;

    public GameObject godHUD;
    
    public bool GODMODE;
    
    public int BasicLifeTime = 5;
    public int FastLifeTime = 3;
    public int SlowLifeTime = 7;

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
        HexGrid.InitGrid(BasicLifeTime, FastLifeTime, SlowLifeTime);



        toggleScript = GetComponent<ToggleScript>();
        if (!GODMODE)
        {
            godHUD.SetActive(false);
        }
        
        gridWidth = HexGrid.gridWidth;
        gridHeight = HexGrid.gridHeight;
        tileScale = HexGrid.tileScale;
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
