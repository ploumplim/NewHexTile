using System;
using Editor;
using UnityEditor;
using UnityEngine;

public class HexCreatorTool : EditorWindow
{
    private string nameLevel;
    private string xCoord = "0";
    private string yCoord = "0";
    private LevelExotic selectedLevelExotic;
    private LevelExotic[] levelExoticAssets;
    private string[] levelExoticNames;
    private HexagonTile.TileStates[,] tileStates;

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
        }
        else
        {
            selectedLevelExotic = null;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Hex Creator Tool", EditorStyles.boldLabel);

        // Section 1: Create new LevelExotic
        GUILayout.BeginVertical("box");
        GUILayout.Label("Create New LevelExotic", EditorStyles.boldLabel);
        nameLevel = EditorGUILayout.TextField("Level Name", nameLevel);
        xCoord = EditorGUILayout.TextField("X", xCoord);
        yCoord = EditorGUILayout.TextField("Y", yCoord);

        if (GUILayout.Button("Create LevelExotic"))
        {
            CreateLevelExotic();
        }
        GUILayout.EndVertical();

        // Section 2: Select existing LevelExotic
        GUILayout.BeginVertical("box");
        GUILayout.Label("Select Existing LevelExotic", EditorStyles.boldLabel);

        if (levelExoticAssets.Length > 0)
        {
            int selectedIndex = EditorGUILayout.Popup("Select LevelExotic", Array.IndexOf(levelExoticAssets, selectedLevelExotic), levelExoticNames);
            selectedLevelExotic = levelExoticAssets[selectedIndex];
        }
        else
        {
            EditorGUILayout.Popup("Select LevelExotic", 0, new string[] { "No LevelExotic available" });
        }

        if (GUILayout.Button("Refresh"))
        {
            LoadLevelExoticAssets();
        }

        if (selectedLevelExotic != null)
        {
            GUILayout.Label($"Selected Level: {selectedLevelExotic.name}");
            DisplayTileStateGrid();
        }
        GUILayout.EndVertical();
    }

    private void CreateLevelExotic()
    {
        if (int.TryParse(xCoord, out int x) && int.TryParse(yCoord, out int y))
        {
            // Create a new LevelExotic ScriptableObject
            LevelExotic newLevelExotic = CreateInstance<LevelExotic>();
            newLevelExotic.gridX = x;
            newLevelExotic.gridY = y;
            newLevelExotic.levelName = nameLevel;

            // Save the new LevelExotic ScriptableObject as an asset
            string assetPath = $"Assets/{nameLevel}.asset";
            AssetDatabase.CreateAsset(newLevelExotic, assetPath);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(newLevelExotic);

            Debug.Log($"New LevelExotic created and updated with X: {x}, Y: {y}, Name: {nameLevel}");

            // Reload the list of LevelExotic assets
            LoadLevelExoticAssets();
        }
        else
        {
            Debug.LogError("Invalid coordinates entered.");
        }
    }

    private void DisplayTileStateGrid()
    {
        if (tileStates == null || tileStates.GetLength(0) != selectedLevelExotic.gridX || tileStates.GetLength(1) != selectedLevelExotic.gridY)
        {
            tileStates = new HexagonTile.TileStates[selectedLevelExotic.gridX, selectedLevelExotic.gridY];
            for (int i = 0; i < selectedLevelExotic.gridX; i++)
            {
                for (int j = 0; j < selectedLevelExotic.gridY; j++)
                {
                    tileStates[i, j] = HexagonTile.TileStates.DefaultTile; // Initialize with a default state
                }
            }
        }

        for (int i = 0; i < selectedLevelExotic.gridX; i++)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < selectedLevelExotic.gridY; j++)
            {
                tileStates[i, j] = (HexagonTile.TileStates)EditorGUILayout.EnumPopup(tileStates[i, j]);
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Tile States"))
        {
            SaveTileStates();
        }
    }

    private void SaveTileStates()
    {
        selectedLevelExotic.levelName = nameLevel;
        selectedLevelExotic.tiles.Clear();
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
        EditorUtility.SetDirty(selectedLevelExotic);
        AssetDatabase.SaveAssets();
        Debug.Log("Tile states and level name saved to LevelExotic.");
    }
}