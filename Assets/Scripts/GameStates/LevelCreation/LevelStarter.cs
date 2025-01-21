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
                // Initialize or load the selected level if needed
                selectedLevelExotic = levelManager.levels.Count > 0 ? levelManager.levels[0] : null;
            }
            else
            {
                Debug.LogWarning("LevelManager is not assigned.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Access levelManager and selectedLevelExotic here
            if (levelManager != null && selectedLevelExotic != null)
            {
                Debug.Log($"Selected Level: {selectedLevelExotic.levelName}");
            }
        }
    }
}