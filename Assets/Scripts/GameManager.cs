using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [HideInInspector]
    public States currentState;
    // This is the hexagon grid that we will be using to store the tiles.
    public HexagonTile[,] Tiles;
    
    // These variables store public information.
    [HideInInspector] //lifetimes of the different tiles.
    public int greenLifeTime;
    [HideInInspector]
    public int blueLifeTime;
    [HideInInspector]
    public int redLifeTime;
    [FormerlySerializedAs("redFusionLifeTime")] [HideInInspector]
    public int redImproveTime;
    [FormerlySerializedAs("blueFusionLifeTime")] [HideInInspector]
    public int blueImproveTime;
    [FormerlySerializedAs("greenFusionLifeTime")] [HideInInspector]
    public int greenImproveTime;
    [HideInInspector] // This is a list of all the living tiles in the grid.
    public List<HexagonTile> livingTiles;
    [HideInInspector] // this is a list of all tiles that can legalize
    public List<HexagonTile> lawfulTiles;
    // This is the script that manages godmode.
    [FormerlySerializedAs("toggleScript")] [HideInInspector]
    public GodModeToggleScript godModeToggleScript;
    // public NextTileSelectorToggle nextTileSelectorToggle;
    
    
    //THIS SECTION HAS ALL THE VARIABLES THAT CAN BE CHANGED IN THE INSPECTOR.
    [Header("Initialized Variables")]
    // This is the hexagon grid that we will be using to store the tiles.
        public HexagonGrid hexGrid;
        // This is the toggle that enables godmode.
        public Toggle godModeToggle;
        // This is the HUD that will be displayed when godmode is enabled.
            public GameObject godHUD;
            
            
        [FormerlySerializedAs("nextTilePreview")]
        [Tooltip("GameObject that will display the next tile to be placed.")]
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
    
    
    [Space]
    
    [Header("Miscellaneous Game Variables")]// This is the next tile that will be placed, randomly generated at the end of each turn.
    [Tooltip("Activate godmode")]
    public bool GODMODE;
    [Tooltip("This is the text that will be displayed when a destroyer tile is previewed.")]
    public string destroyerText = "La Bomba";
    [Tooltip("This is the text that will be displayed when a pakku tile is previewed.")]
    public string pakkuText = "PakkuTile";
    [Tooltip("This is the minimum amount of tiles that need to be placed before we spawn bombs. (Not implemented)")]
    public int destroyerDangerLimit = 4;
    [Header("Tile Weights")]
    public int greenTileWeight = 25;
    public int blueTileWeight = 25;
    public int redTileWeight = 25;
    public int destroyerTileWeight = 25;
    public int pakkuTileWeight = 25;

    [Header("Preview colors")]
    public Color greenTileColor = new Color(0f, 0.5f, 0f);
    public Color blueTileColor = new Color(0f, 0f, 0.5f);
    public Color redTileColor = new Color(0.5f, 0f, 0f);
    public Color destroyerTileColor = new Color(1f, 0f, 1f);
    public Color pakkuTileColor = new Color(1f, 1f, 0f);
    
    [Header("Next Tile Generation")]
    
    // This is the length of the future tile list.
    [Tooltip("This is the amount of future tiles that will be generated when the list is less than 2.")]
    public int futureTilesListCount = 10;
    
    [Space]
    // This list has the next tile states that will be generated.
    public List<HexagonTile.TileStates> futureTileStateList;

    [Space]
    
    //This list has the valid future tile states that can be generated.
    public List<HexagonTile.TileStates> validFutureTileStates = new List<HexagonTile.TileStates>
    {
        HexagonTile.TileStates.GreenTile,
        HexagonTile.TileStates.BlueTile,
        HexagonTile.TileStates.RedTile,
        HexagonTile.TileStates.DestroyerTile,
        HexagonTile.TileStates.PakkuTile
    };
    
    [HideInInspector] public List<int> weights;
    // This is the minimum amount of tiles that need to be placed before we spawn bombs
    
    [Header("Starter Tile")]
    // starter tile position
    public int starterTileXPosition = 1;
    public int starterTileYPosition = 1;
    
    [Tooltip("Only one spreader tile for now.")]
    [Header("Spreader Tile")]
    // spreader tile position
    public int spreaderTileXPosition = 1;
    public int spreaderTileYPosition = 1;
    
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
        
        Tiles = hexGrid.TileInstances;
        
        // Obtain the lifetime of the different tilestates for output.
        greenLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().greenLifeTime - 1;
        redLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().redLifeTime - 1;
        blueLifeTime = Tiles[1, 1].GetComponent<HexagonTile>().blueLifeTime - 1;
        redImproveTime = Tiles[1, 1].GetComponent<HexagonTile>().redImproveValue - 1;
        blueImproveTime = Tiles[1, 1].GetComponent<HexagonTile>().blueImproveValue - 1;
        greenImproveTime = Tiles[1, 1].GetComponent<HexagonTile>().greenImproveValue - 1;
        
        // SET STARTER TILE
        var starterTile = hexGrid.TileInstances[starterTileXPosition, starterTileYPosition].GetComponent<HexagonTile>();
          starterTile.TileStateChange(HexagonTile.TileStates.StarterTile);
        
        // SET SPREADER TILE
        var spreaderTile = hexGrid.TileInstances[spreaderTileXPosition, spreaderTileYPosition].GetComponent<HexagonTile>();
        spreaderTile.TileStateChange(HexagonTile.TileStates.SpreadingTile);
        
        // Set the current state to the placement state.
        currentState = GetComponent<PlacementState>();
        currentState.Enter();
        
        //Debug.Log(starterTile.gameObject.name);
      
        weights = new List<int> {greenTileWeight, blueTileWeight, redTileWeight, destroyerTileWeight, pakkuTileWeight};        

        // Initialize scripts
        godModeToggleScript = GetComponent<GodModeToggleScript>();
        
        
        // nextTileSelectorToggle = GetComponent<NextTileSelectorToggle>();
        
        // Initialize the future tile state list by creating 10 random states and adding them to the list.

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
        // reset the future tile list
        futureTileStateList.Clear();
        RegenerateFutureTileStateList(futureTilesListCount);
        
        // Reset the game state
        currentState = GetComponent<CountersState>();
        currentState.Enter();
        
        // Set the starter tile
        var starterTile = hexGrid.TileInstances[starterTileXPosition, starterTileYPosition].GetComponent<HexagonTile>();
        starterTile.TileStateChange(HexagonTile.TileStates.StarterTile);
        
        // reset the spreader generation of all my tiles
        foreach (HexagonTile tile in Tiles)
        {
            tile.spreaderGeneration = 1;
        }
        
        
        // Set the spreader tile
        var spreaderTile = hexGrid.TileInstances[spreaderTileXPosition, spreaderTileYPosition].GetComponent<HexagonTile>();
        spreaderTile.TileStateChange(HexagonTile.TileStates.SpreadingTile);
        
        
    }

    public void RegenerateFutureTileStateList(int count)
    {
        futureTileStateList.Clear();
        System.Random random = new System.Random();

        for (int i = 0; i < count; i++)
        {
            int totalWeight = weights.Sum();
            int randomValue = random.Next(totalWeight);
            int cumulativeWeight = 0;

            for (int j = 0; j < validFutureTileStates.Count; j++)
            {
                cumulativeWeight += weights[j];
                if (randomValue < cumulativeWeight)
                {
                    futureTileStateList.Add(validFutureTileStates[j]);
                    break;
                }
            }
        }
    }
    public void SwapIncomingTiles()
    {
        // Here, we change the first element of the future tile list with the second one.
        if (futureTileStateList.Count > 1)
        {
            // Swap the first and second elements of the future tile list.
            (futureTileStateList[0], futureTileStateList[1]) = (futureTileStateList[1], futureTileStateList[0]);
        }
        CorrectTileUIPreviews(0);
        CorrectTileUIPreviews(1);
    }

    public void CorrectTileUIPreviews(int index)
    {
        //This function calls the visual representation of the next tile to be placed on the board.
        // TODO: Implement the correct color and text for all tile choices on an appropriate UI element.

        if (index == 0)
        {
            switch (futureTileStateList[index])
            {
                case HexagonTile.TileStates.GreenTile:
                    nextTilePreview1.GetComponentInChildren<Image>().color = greenTileColor;
                    nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + greenLifeTime + ", +" + greenImproveTime;
                    break;
                case HexagonTile.TileStates.BlueTile:
                    nextTilePreview1.GetComponentInChildren<Image>().color = blueTileColor;
                    nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + blueLifeTime + ", +" + blueImproveTime;
                    break;
                case HexagonTile.TileStates.RedTile:
                    nextTilePreview1.GetComponentInChildren<Image>().color = redTileColor;
                    nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + redLifeTime + ", +" + redImproveTime;
                    break;
                case HexagonTile.TileStates.DestroyerTile:
                    nextTilePreview1.GetComponentInChildren<Image>().color = destroyerTileColor;
                    nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = destroyerText;
                    break;
                case HexagonTile.TileStates.PakkuTile:
                    nextTilePreview1.GetComponentInChildren<Image>().color = pakkuTileColor;
                    nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = pakkuText;
                    break;
            }
        }

        if (index == 1)
        {
            switch (futureTileStateList[index])
            {
                case HexagonTile.TileStates.GreenTile:
                    nextTilePreview2.GetComponentInChildren<Image>().color = greenTileColor;
                    break;
                case HexagonTile.TileStates.BlueTile:
                    nextTilePreview2.GetComponentInChildren<Image>().color = blueTileColor;
                    break;
                case HexagonTile.TileStates.RedTile:
                    nextTilePreview2.GetComponentInChildren<Image>().color = redTileColor;
                    break;
                case HexagonTile.TileStates.DestroyerTile:
                    nextTilePreview2.GetComponentInChildren<Image>().color = destroyerTileColor;
                    break;
                case HexagonTile.TileStates.PakkuTile:
                    nextTilePreview2.GetComponentInChildren<Image>().color = pakkuTileColor;
                    break;
            }
            
        }
       
    }

    
}
