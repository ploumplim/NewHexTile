using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public States currentState;

    public HexagonGrid HexGrid;
    
    public GameObject[,] Tiles;
    public int gridWidth;
    public int gridHeight;
    public float tileScale;
    
    public GameObject[] livingTiles;

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
