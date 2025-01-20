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
            LoadTileStatesFromSelectedLevel();
        }
        else
        {
            selectedLevelExotic = null;
        }
    }

    private void LoadTileStatesFromSelectedLevel()
    {
        if (selectedLevelExotic == null) return;

        int gridX = selectedLevelExotic.gridX;
        int gridY = selectedLevelExotic.gridY;
        tileStates = new HexagonTile.TileStates[gridX, gridY];

        // Initialize tileStates with data from the selectedLevelExotic
        foreach (var tile in selectedLevelExotic.tiles)
        {
            tileStates[tile.x, tile.y] = tile.tileState;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Hex Creator Tool", EditorStyles.boldLabel);

        // Button to create new LevelExotic
        if (GUILayout.Button("Create LevelExotic"))
        {
            ShowCreateLevelExoticPopup();
        }

        // Section 2: Select existing LevelExotic
        GUILayout.BeginVertical("box");
        GUILayout.Label("Select Existing LevelExotic", EditorStyles.boldLabel);

        if (levelExoticAssets.Length > 0)
        {
            int selectedIndex = Array.IndexOf(levelExoticAssets, selectedLevelExotic);
            int newIndex = EditorGUILayout.Popup("Select LevelExotic", selectedIndex, levelExoticNames);

            if (newIndex != selectedIndex)
            {
                selectedLevelExotic = levelExoticAssets[newIndex];
                LoadTileStatesFromSelectedLevel(); // Update tileStates to match the selected level
            }
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

    private void ShowCreateLevelExoticPopup()
    {
        nameLevel = EditorUtility.DisplayDialogComplex("Create New LevelExotic", "Enter Level Name:", "Create", "Cancel", null) == 0 ? nameLevel : null;
        xCoord = EditorUtility.DisplayDialogComplex("Create New LevelExotic", "Enter X Coordinate:", "Create", "Cancel", null) == 0 ? xCoord : null;
        yCoord = EditorUtility.DisplayDialogComplex("Create New LevelExotic", "Enter Y Coordinate:", "Create", "Cancel", null) == 0 ? yCoord : null;

        if (!string.IsNullOrEmpty(nameLevel) && !string.IsNullOrEmpty(xCoord) && !string.IsNullOrEmpty(yCoord))
        {
            CreateLevelExotic();
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

            LoadLevelExoticAssets();
        }
        else
        {
            Debug.LogError("Invalid coordinates entered.");
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

        if (GUILayout.Button("Save Tile States"))
        {
            SaveTileStates();
        }
    }

    private void SaveTileStates()
    {
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
        Debug.Log("Tile states saved to LevelExotic.");
    }
}
