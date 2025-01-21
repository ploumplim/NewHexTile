using System.Collections.Generic;
using GameStates;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(LevelStarter))]
    public class LevelStarterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LevelStarter levelStarter = (LevelStarter)target;

            // Draw the default inspector
            DrawDefaultInspector();

            if (levelStarter.levelManager != null)
            {
                List<string> levelNames = new List<string>();
                foreach (var level in levelStarter.levelManager.levels)
                {
                    levelNames.Add(level.name);
                }

                int selectedIndex = levelStarter.levelManager.levels.IndexOf(levelStarter.selectedLevelExotic);
                selectedIndex = EditorGUILayout.Popup("Select Level", selectedIndex, levelNames.ToArray());

                if (selectedIndex >= 0 && selectedIndex < levelStarter.levelManager.levels.Count)
                {
                    levelStarter.selectedLevelExotic = levelStarter.levelManager.levels[selectedIndex];
                }
            }
            else
            {
                EditorGUILayout.HelpBox("LevelManager is not assigned.", MessageType.Warning);
            }

            // Save changes
            if (GUI.changed)
            {
                EditorUtility.SetDirty(levelStarter);
            }
        }
    }
}