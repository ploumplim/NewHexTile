using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    public GameObject gridParent;

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
                        tileState.ApplyState(hexTile, TileState.TileStates.BasicState);
                        hexTile.ApplyLifeTime(hexTile); // Apply the lifetime to the tile
                        hexTile.DecrementLifeTimeForAllTiles(gridParent);
                    }
                }
            }
        }
    }
}