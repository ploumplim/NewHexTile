using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Disable the delete button
        GUI.enabled = false;
        if (GUILayout.Button("Delete"))
        {
            // Do nothing
        }
        GUI.enabled = true;
    }
}