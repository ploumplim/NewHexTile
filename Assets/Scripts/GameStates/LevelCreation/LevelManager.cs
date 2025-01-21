// LevelManager.cs
using System.Collections.Generic;
using Editor;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelManager", menuName = "LevelManager", order = 0)]
public class LevelManager : ScriptableObject
{
    public List<LevelExotic> levels = new List<LevelExotic>();

    public void UpdateLevel(LevelExotic updatedLevel)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].name == updatedLevel.name)
            {
                levels[i] = updatedLevel;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                return;
            }
        }
        
        Debug.LogWarning($"Level {updatedLevel.name} not found in LevelManager.");
    }
    
}