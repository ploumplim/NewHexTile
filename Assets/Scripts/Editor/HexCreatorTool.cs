using System;
using Editor;
using UnityEditor;
using UnityEngine;

public class HexCreatorTool : EditorWindow
{
    private LevelExotic selectedLevelExotic;
    private LevelExotic[] levelExoticAssets;
    private string[] levelExoticNames;
    private HexagonTile.TileStates[,] tileStates;
    private bool[] tileStateSelections;

    [MenuItem("Tools/Hex Creator Tool")]
    public static void ShowWindow()
    {
        GetWindow<HexCreatorTool>("Hex Creator Tool");
    }

    private void OnEnable()
    {
        LoadLevelExoticAssets();
    }

    private void LoadLevelExoticAssets()
    {
        string[] guids = AssetDatabase.FindAssets("t:LevelExotic");
        levelExoticAssets = new LevelExotic[guids.Length];
        levelExoticNames = new string[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            levelExoticAssets[i] = AssetDatabase.LoadAssetAtPath<LevelExotic>(path);
            levelExoticNames[i] = levelExoticAssets[i].name;
        }

        if (levelExoticAssets.Length > 0)
        {
            selectedLevelExotic = levelExoticAssets[0];
            LoadTileStates();
            LoadTileStateSelections();
        }
        else
        {
            selectedLevelExotic = null;
        }
    }

    private void LoadTileStates()
    {
        if (selectedLevelExotic == null) return;

        int gridX = selectedLevelExotic.gridX;
        int gridY = selectedLevelExotic.gridY;
        tileStates = new HexagonTile.TileStates[gridX, gridY];

        foreach (var tile in selectedLevelExotic.tiles)
        {
            tileStates[tile.x, tile.y] = tile.tileState;
        }
    }

    private void LoadTileStateSelections()
    {
        if (selectedLevelExotic == null) return;

        int tileStateCount = Enum.GetValues(typeof(HexagonTile.TileStates)).Length;
        tileStateSelections = new bool[tileStateCount];

        foreach (var state in selectedLevelExotic.hexagonTileStates)
        {
            tileStateSelections[(int)state] = true;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Hex Creator Tool", EditorStyles.boldLabel);

        // Red block for creating new LevelExotic
        GUIStyle redStyle = new GUIStyle(GUI.skin.box);
        redStyle.normal.background = MakeTex(2, 2, new Color(1f, 0.5f, 0.5f, 1f));
        GUILayout.BeginVertical(redStyle);
        if (GUILayout.Button("Create LevelExotic"))
        {
            CreateLevelExoticPopup.ShowPopup();
        }
        GUILayout.EndVertical();

        // Blue block for selecting existing LevelExotic
        GUIStyle blueStyle = new GUIStyle(GUI.skin.box);
        blueStyle.normal.background = MakeTex(2, 2, new Color(0.5f, 0.5f, 1f, 1f));
        GUILayout.BeginVertical(blueStyle);
        GUILayout.Label("Select Existing LevelExotic", EditorStyles.boldLabel);
        GUILayout.Space(20);

        if (levelExoticAssets.Length > 0)
        {
            int selectedIndex = EditorGUILayout.Popup("Select LevelExotic", Array.IndexOf(levelExoticAssets, selectedLevelExotic), levelExoticNames);
            if (selectedIndex != -1 && selectedLevelExotic != levelExoticAssets[selectedIndex])
            {
                selectedLevelExotic = levelExoticAssets[selectedIndex];
                LoadTileStates();
                LoadTileStateSelections();
            }
        }
        else
        {
            EditorGUILayout.Popup("Select LevelExotic", 0, new string[] { "No LevelExotic available" });
        }
        GUILayout.Space(20);

        if (selectedLevelExotic != null)
        {
            GUILayout.Label($"Selected Level: {selectedLevelExotic.name}");
            DisplayTileStateGrid();
        }
        GUILayout.EndVertical();

        // Add space between the menus
        GUILayout.Space(20);

        // Refresh button
        if (GUILayout.Button("Refresh"))
        {
            LoadLevelExoticAssets();
        }
    }

    private void DisplayTileStateGrid()
    {
        if (tileStates == null) return;
        for (int i = 0; i < selectedLevelExotic.gridX; i++)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < selectedLevelExotic.gridY; j++)
            {
                tileStates[i, j] = (HexagonTile.TileStates)EditorGUILayout.EnumPopup(tileStates[i, j]);
            }
            GUILayout.EndHorizontal();
        }

        // Add space between the grid and the "Select Valid Tile States:" label
        GUILayout.Space(20);

        GUILayout.Label("Select Valid Tile States:", EditorStyles.boldLabel);
        for (int i = 0; i < tileStateSelections.Length; i++)
        {
            tileStateSelections[i] = EditorGUILayout.Toggle(((HexagonTile.TileStates)i).ToString(), tileStateSelections[i]);
        }
        GUILayout.Space(20);

        if (GUILayout.Button("Save Tile States"))
        {
            SaveTileStates();
        }
    }

    private void SaveTileStates()
    {
        
        
        selectedLevelExotic.tiles.Clear();
        selectedLevelExotic.hexagonTileStates.Clear();

        for (int i = 0; i < selectedLevelExotic.gridX; i++)
        {
            for (int j = 0; j < selectedLevelExotic.gridY; j++)
            {
                TileInfo tileInfo = new TileInfo
                {
                    x = i,
                    y = j,
                    tileState = tileStates[i, j]
                };
                selectedLevelExotic.tiles.Add(tileInfo);
            }
        }

        for (int i = 0; i < tileStateSelections.Length; i++)
        {
            if (tileStateSelections[i])
            {
                selectedLevelExotic.hexagonTileStates.Add((HexagonTile.TileStates)i);
            }
        }

        EditorUtility.SetDirty(selectedLevelExotic);
        AssetDatabase.SaveAssets();
        Debug.Log("Tile states and valid tile states saved to LevelExotic.");
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}

public class CreateLevelExoticPopup : EditorWindow
{
    private string nameLevel;
    private string xCoord = "0";
    private string yCoord = "0";

    public static void ShowPopup()
    {
        var window = GetWindow<CreateLevelExoticPopup>("Create LevelExotic");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New LevelExotic", EditorStyles.boldLabel);
        nameLevel = EditorGUILayout.TextField("Level Name", nameLevel);
        xCoord = EditorGUILayout.TextField("X", xCoord);
        yCoord = EditorGUILayout.TextField("Y", yCoord);

        if (GUILayout.Button("Create"))
        {
            CreateLevelExotic();
            Close();
        }
    }

    private void CreateLevelExotic()
    {
        if (int.TryParse(xCoord, out int x) && int.TryParse(yCoord, out int y))
        {
            LevelExotic newLevelExotic = CreateInstance<LevelExotic>();
            newLevelExotic.gridX = x;
            newLevelExotic.gridY = y;
            newLevelExotic.levelName = nameLevel;

            string assetPath = $"Assets/{nameLevel}.asset";
            AssetDatabase.CreateAsset(newLevelExotic, assetPath);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(newLevelExotic);

            Debug.Log($"New LevelExotic created with X: {x}, Y: {y}, Name: {nameLevel}");
        }
        else
        {
            Debug.LogError("Invalid coordinates entered.");
        }
    }
}