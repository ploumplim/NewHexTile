using System.Collections;
using System.Collections.Generic;
using Editor;
using UnityEngine;
using TMPro;

public class Ui_ChoiceLevelDropDown : MonoBehaviour
{
    public TMP_Dropdown levelDropdown;
    public LevelManager levelManager;
    public HexagonGrid hexagonGrid;

    private int currentLevelIndex = -1;

    void Start()
    {
        PopulateDropdown();
        SetInitialLevel();
        levelDropdown.onValueChanged.AddListener(DropDownSample);
    }

    private void PopulateDropdown()
    {
        List<string> levelNames = new List<string>();
        foreach (var level in levelManager.levels)
        {
            levelNames.Add(level.levelName);
        }
        levelDropdown.ClearOptions();
        levelDropdown.AddOptions(levelNames);
    }

    private void SetInitialLevel()
    {
        LevelExotic currentLevel = hexagonGrid.LevelData.selectedLevelExotic;
        if (currentLevel != null)
        {
            int index = levelManager.levels.IndexOf(currentLevel);
            if (index >= 0)
            {
                levelDropdown.value = index;
                currentLevelIndex = index;
            }
        }
    }

    public void DropDownSample(int index)
    {
        if (index >= 0 && index < levelManager.levels.Count)
        {
            if (index != currentLevelIndex)
            {
                if (currentLevelIndex != -1)
                {
                    UnloadLevel();
                }
                LoadLevel(index);
                currentLevelIndex = index;
            }
        }
        else
        {
            Debug.LogWarning("Selected index is out of range.");
        }
    }

    private void LoadLevel(int index)
    {
        LevelExotic selectedLevel = levelManager.levels[index];
        hexagonGrid.LevelData.selectedLevelExotic = selectedLevel;
        hexagonGrid.InitGrid();
        hexagonGrid.Grid.transform.position = hexagonGrid.GridAnchor.transform.position;
    }

    private void UnloadLevel()
    {
        hexagonGrid.ClearGrid();
        hexagonGrid.Grid.transform.position = Vector3.zero; // Reset position to ensure it updates correctly
    }
}