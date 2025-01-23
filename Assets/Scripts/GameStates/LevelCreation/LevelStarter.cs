using Editor;
using UnityEngine;

namespace GameStates
{
    public class LevelStarter : MonoBehaviour
    {
        public LevelManager levelManager;
        [HideInInspector]
        public LevelExotic selectedLevelExotic;

        // Start is called before the first frame update
        void Start()
        {
            if (levelManager != null)
            {
                // Find the level by its name or other unique property
                selectedLevelExotic = levelManager.levels.Find(level => level.name == selectedLevelExotic.name);
                if (selectedLevelExotic == null && levelManager.levels.Count > 0)
                {
                    selectedLevelExotic = levelManager.levels[0]; // Fallback to the first level if not found
                }
            }
            else
            {
                Debug.LogWarning("LevelManager is not assigned.");
            }
        }
    }
}