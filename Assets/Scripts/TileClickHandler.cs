using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    public GameObject gridParent;
    public ToggleScript toggleScript; // Reference to the ToggleScript

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                HexagonTile hexTile = hit.collider.GetComponent<HexagonTile>();
                if (hexTile != null)
                {
                    TileState tileState = hexTile.GetComponent<TileState>();
                    if (tileState != null && tileState.currentState == TileState.TileStates.LegalState)
                    {
                        // Get the active tile index from the ToggleScript
                        int activeTileIndex = toggleScript.activeTile;

                        // Apply the corresponding state based on the active tile index
                        switch (activeTileIndex)
                        {
                            case 0:
                                tileState.ApplyState(hexTile, TileState.TileStates.BasicState);
                                Debug.Log("BasicState");
                                break;
                            case 1:
                                tileState.ApplyState(hexTile, TileState.TileStates.SlowState);
                                Debug.Log("SlowState");
                                break;
                            case 2:
                                tileState.ApplyState(hexTile, TileState.TileStates.FastState);
                                Debug.Log("FastState");
                                break;
                            default:
                                Debug.LogError("Invalid active tile index: " + activeTileIndex);
                                break;
                        }

                        // Apply the lifetime to the tile
                        hexTile.DecrementLifeTimeForAllTiles(gridParent);
                    }
                }
            }
        }
    }
}