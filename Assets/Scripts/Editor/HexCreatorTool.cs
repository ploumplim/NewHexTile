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
    private Vector2 scrollPosition;

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
    GUI.backgroundColor = Color.red;
    if (GUILayout.Button("Create Level"))
    {
        CreateLevelExoticPopup.ShowPopup();
    }
    GUI.backgroundColor = Color.white;
    GUILayout.Space(10);
    GUI.backgroundColor = Color.cyan;
    if (GUILayout.Button("Refresh Level List & Data"))
    {
        LoadLevelExoticAssets();
    }
    GUI.backgroundColor = Color.white;
    GUILayout.Space(10);
    GUI.backgroundColor = Color.green;
    if (GUILayout.Button("Save Tile States"))
    {
        SaveTileStates();
    }
    GUI.backgroundColor = Color.white;
    GUILayout.EndVertical();

    // Blue block for selecting existing LevelExotic
    GUIStyle blueStyle = new GUIStyle(GUI.skin.box);
    blueStyle.normal.background = MakeTex(2, 2, new Color(0.5f, 0.5f, 1f, 1f));
    GUILayout.BeginVertical(blueStyle);

    GUILayout.BeginHorizontal();
    GUILayout.FlexibleSpace();
    GUILayout.Label("Select Level", EditorStyles.boldLabel);
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();
    GUILayout.Space(10);

    if (levelExoticAssets.Length > 0)
    {
        int selectedIndex = EditorGUILayout.Popup("Select Level :", Array.IndexOf(levelExoticAssets, selectedLevelExotic), levelExoticNames);
        if (selectedIndex != -1 && selectedLevelExotic != levelExoticAssets[selectedIndex])
        {
            selectedLevelExotic = levelExoticAssets[selectedIndex];
            LoadTileStates();
            LoadTileStateSelections();
        }
    }
    else
    {
        EditorGUILayout.Popup("Select Level ", 0, new string[] { "No Level available" });
    }
    GUILayout.Space(20);

    if (selectedLevelExotic != null)
    {
       // GUILayout.Label($"Modify parameters of level : {selectedLevelExotic.name}");
        
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label($"Modify parameters of level : {selectedLevelExotic.name}", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        
        // Add text field for modifying the level name
        selectedLevelExotic.levelName = EditorGUILayout.TextField("Level Name", selectedLevelExotic.levelName);

        // Add text fields for modifying x and y values
        selectedLevelExotic.gridX = EditorGUILayout.IntField("Grid X", selectedLevelExotic.gridX);
        selectedLevelExotic.gridY = EditorGUILayout.IntField("Grid Y", selectedLevelExotic.gridY);
        GUILayout.Space(20);

        // Box for DisplayTileStateGrid with scroll view
         DisplayTileStateGrid();
       
    }
    GUILayout.EndVertical();

    // Add space between the menus
    GUILayout.Space(10);

    // Refresh button
}
    private void DisplayTileStateGrid()
{
    if (tileStates == null) return;

    // Définir les dimensions visibles pour la grille
    float visibleWidth = EditorGUIUtility.currentViewWidth - 40; // Ajuste la largeur visible
    float visibleHeight = 300f; // Ajuste la hauteur visible

    // Dimensions totales de la grille
    float totalWidth = selectedLevelExotic.gridY * 150; // 150 est la largeur estimée de chaque EnumPopup
    float totalHeight = selectedLevelExotic.gridX * 20; // 20 est la hauteur estimée de chaque EnumPopup

    // Créer un conteneur avec des barres de défilement
    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(visibleWidth), GUILayout.Height(visibleHeight));
    GUILayout.BeginHorizontal(GUILayout.Width(totalWidth));

    for (int i = 0; i < selectedLevelExotic.gridX; i++)
    {
        GUILayout.BeginVertical(GUILayout.Height(totalHeight));
        for (int j = 0; j < selectedLevelExotic.gridY; j++)
        {
            // Ensure i and j are within the bounds of the array
            if (i >= 0 && i < tileStates.GetLength(0) && j >= 0 && j < tileStates.GetLength(1))
            {
                tileStates[i, j] = (HexagonTile.TileStates)EditorGUILayout.EnumPopup(tileStates[i, j], GUILayout.Width(140));
            }
        }
        GUILayout.EndVertical();
    }

    GUILayout.EndHorizontal();
    EditorGUILayout.EndScrollView();

    // Ajouter un peu d'espace pour séparer des autres éléments
    GUILayout.Space(10);

    GUILayout.BeginHorizontal();
    GUILayout.FlexibleSpace();
    GUILayout.Label("Select Valid Tile States:", EditorStyles.boldLabel);
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    int maxItemsPerRow = Mathf.FloorToInt(EditorGUIUtility.currentViewWidth / 150); // Ajuste 150 en fonction de la largeur de chaque checkbox
    int currentItem = 0;
    GUILayout.Space(20);
    GUILayout.BeginHorizontal();
    for (int i = 0; i < tileStateSelections.Length; i++)
    {
        if (currentItem >= maxItemsPerRow)
        {
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            currentItem = 0;
        }

        tileStateSelections[i] = EditorGUILayout.Toggle(((HexagonTile.TileStates)i).ToString(), tileStateSelections[i]);
        currentItem++;
    }
    GUILayout.EndHorizontal();
}


   private void SaveTileStates()
{
    // Apply changes to the level name and grid dimensions
   
    selectedLevelExotic.levelName = EditorGUILayout.TextField("Level Name", selectedLevelExotic.levelName);
    selectedLevelExotic.gridX = EditorGUILayout.IntField("Grid X", selectedLevelExotic.gridX);
    selectedLevelExotic.gridY = EditorGUILayout.IntField("Grid Y", selectedLevelExotic.gridY);
    string newName = selectedLevelExotic.levelName;

    selectedLevelExotic.tiles.Clear();
    selectedLevelExotic.hexagonTileStates.Clear();

    for (int i = 0; i < selectedLevelExotic.gridX; i++)
    {
        for (int j = 0; j < selectedLevelExotic.gridY; j++)
        {
            if (tileStates[i, j] != HexagonTile.TileStates.DefaultTile)
            {
                selectedLevelExotic.tiles.Add(new TileInfo { x = i, y = j, tileState = tileStates[i, j] });
            }
        }
    }

    for (int i = 0; i < tileStateSelections.Length; i++)
    {
        if (tileStateSelections[i])
        {
            selectedLevelExotic.hexagonTileStates.Add((HexagonTile.TileStates)i);
        }
    }

    // Rename the asset if the name has changed
    if (selectedLevelExotic == null || string.IsNullOrEmpty(newName))
    {
        Debug.LogWarning("Selected ScriptableObject or newName is null/empty.");
        return;
    }

    if (newName != selectedLevelExotic.name)
    {
        string assetPath = AssetDatabase.GetAssetPath(selectedLevelExotic);

        // Update the name in the ScriptableObject and rename the asset
        selectedLevelExotic.name = newName;
        AssetDatabase.RenameAsset(assetPath, newName);

        // Optional: Save changes to the asset
        EditorUtility.SetDirty(selectedLevelExotic);
        AssetDatabase.SaveAssets();

        // Rename the GameObject associated with the ScriptableObject if it exists
        GameObject levelGameObject = GameObject.Find(selectedLevelExotic.name);
        if (levelGameObject != null)
        {
            levelGameObject.name = newName;
        }

        Debug.Log($"Successfully renamed ScriptableObject to '{newName}' and updated GameObject if applicable.");
    }
    else
    {
        Debug.Log("The new name is the same as the current name. No renaming performed.");
    }

    EditorUtility.SetDirty(selectedLevelExotic);
    AssetDatabase.SaveAssets();
    Debug.Log("Tile states and valid tile states saved to Level.");

    // Update the level in LevelManager
    LevelManager levelManager = AssetDatabase.LoadAssetAtPath<LevelManager>("Assets/LevelManager.asset");
    if (levelManager != null)
    {
        levelManager.UpdateLevel(selectedLevelExotic);
    }
    else
    {
        Debug.LogError("LevelManager asset not found at 'Assets/LevelManager.asset'");
    }
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

    public new static void ShowPopup()
    {
        var window = GetWindow<CreateLevelExoticPopup>("Create Level");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Level", EditorStyles.boldLabel);
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

            string assetPath = $"Assets/Level/{nameLevel}.asset";
            AssetDatabase.CreateAsset(newLevelExotic, assetPath);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(newLevelExotic);

            // Add the new LevelExotic to the LevelManager
            LevelManager levelManager = AssetDatabase.LoadAssetAtPath<LevelManager>("Assets/LevelManager.asset");
            if (levelManager != null)
            {
                levelManager.levels.Add(newLevelExotic);
                EditorUtility.SetDirty(levelManager);
                AssetDatabase.SaveAssets();
            }
            else
            {
                Debug.LogError("LevelManager asset not found at 'Assets/LevelManager.asset'");
            }

            Debug.Log($"New LevelExotic created with X: {x}, Y: {y}, Name: {nameLevel}");
        }
        else
        {
            Debug.LogError("Invalid coordinates entered.");
        }
    }
}