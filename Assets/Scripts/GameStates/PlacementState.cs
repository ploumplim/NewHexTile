using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementState : States
{

    private HexagonTile previousHexTile;
    HexagonTile MaxNeighboor = null; 
    public override void Enter()
    {
        // Generate the next tile to be placed on the board by updating the next tile state to the first element in the future tile list.
        
        // Generate a preview of the next two tiles to be placed on the board.
        GM.CorrectTileUIPreviews(0);
        GM.CorrectTileUIPreviews(1);
    }
    
    public override void Tick()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out RaycastHit hit))
    {
        HexagonTile hexTile = hit.collider.GetComponent<HexagonTile>();

        if (hexTile != null)
        {
            HandleMouseClick(hexTile);
            HandleTileHover(hexTile);
        }
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
        GM.SwapIncomingTiles();
    }
}

    private void HandleMouseClick(HexagonTile hexTile)
    {
        if (Input.GetMouseButtonDown(0) && hexTile.currentTileState == HexagonTile.TileStates.LegalTile)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.clickSound, this.transform.position);

            if (GM.GODMODE)
            {
                ApplyGodModeTileState(hexTile);
            }
            else
            {
                NextTileCreate(hexTile);
            }
            
            hexTile.TargetTileEffect();

            GM.ChangeState(GM.GetComponent<EffectState>());
        }
        else if (Input.GetMouseButtonDown(0) 
                 && hexTile.currentTileState == HexagonTile.TileStates.TargetTile
                 && hexTile.isTargetable)
        {
            Debug.Log("Victory");
        }
        
    }

private void ApplyGodModeTileState(HexagonTile hexTile)
{
    int activeTileIndex = GM.godModeToggleScript.activeTile;
    switch (activeTileIndex)
    {
        case 0:
            NextTileCreate(hexTile);
            break;
        case 1:
            hexTile.TileStateChange(HexagonTile.TileStates.GreenTile);
            break;
        case 2:
            hexTile.TileStateChange(HexagonTile.TileStates.BlueTile);
            break;
        case 3:
            hexTile.TileStateChange(HexagonTile.TileStates.RedTile);
            break;
        case 4:
            hexTile.TileStateChange(HexagonTile.TileStates.DestroyerTile);
            break;
        case 5:
            hexTile.TileStateChange(HexagonTile.TileStates.PakkuTile);
            break;
        default:
            Debug.LogError("Invalid active tile index: " + activeTileIndex);
            break;
    }
}

private void HandleTileHover(HexagonTile hexTile)
{
    if (hexTile != previousHexTile)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.hoverSound, this.transform.position);
        ResetPreviousTileVisuals();
        previousHexTile = hexTile;
    }

    if (hexTile.currentTileState == HexagonTile.TileStates.LegalTile)
    {
        UpdateTileVisuals(hexTile);
    }
}

private void ResetPreviousTileVisuals()
{
    if (previousHexTile != null 
        && previousHexTile.currentTileState == HexagonTile.TileStates.LegalTile)
    {
        foreach (var visual in previousHexTile.tileVisuals)
        {
            visual.SetActive(false);
        }
        previousHexTile.tileVisuals[1].SetActive(true);
        foreach (var adjacentTile in previousHexTile.GetAdjacentTiles())
        {
            if (adjacentTile.lifeTime > 0
                && adjacentTile.currentTileState != HexagonTile.TileStates.StarterTile)
            {
                if (previousHexTile.willExplode)
                {
                    previousHexTile.tileVisuals[14].SetActive(true);
                }
                adjacentTile.GetComponentInChildren<TextMeshPro>().SetText(adjacentTile.lifeTime.ToString());
                adjacentTile.GetComponentInChildren<TextMeshPro>().color = Color.white;
            }
        }
    }
}

private void UpdateTileVisuals(HexagonTile hexTile)
{
    foreach (var visual in hexTile.tileVisuals)
    {
        visual.SetActive(false);
    }

    foreach (var neighborTile in hexTile.GetAdjacentTiles())
    {
        if (neighborTile.lifeTime > 0 && 
            neighborTile.currentTileState != HexagonTile.TileStates.StarterTile
            &&neighborTile.currentTileState != HexagonTile.TileStates.TargetTile)
        {
            int lifeTimeImproved = neighborTile.lifeTime + GetTileImproveValue(hexTile);
            neighborTile.GetComponentInChildren<TextMeshPro>().SetText(lifeTimeImproved.ToString());
            neighborTile.GetComponentInChildren<TextMeshPro>().color = Color.magenta;
        }
    }

    foreach (var VARIABLE in GM.Tiles)
    {
        if (VARIABLE.currentTileState == HexagonTile.TileStates.PakkuTile)
        {
            foreach (var PakkuNeighboor in VARIABLE.GetAdjacentTiles())
            {
                if (PakkuNeighboor.lifeTime>0)
                {
                    PakkuNeighboor.tileVisuals[13].SetActive(true);
                }
                
            }
        }
    }

    hexTile.tileVisuals[GetTileVisualIndex(GM.futureTileStateList[0])].SetActive(true);
}

private int GetTileImproveValue(HexagonTile hexTile)
{
    switch (GM.futureTileStateList[0])
    {
        case HexagonTile.TileStates.GreenTile:
            return hexTile.greenImproveValue;
        case HexagonTile.TileStates.BlueTile:
            return hexTile.blueImproveValue;
        case HexagonTile.TileStates.RedTile:
            return hexTile.redImproveValue;
        default:
            return 0;
    }
}

private int GetTileVisualIndex(HexagonTile.TileStates state)
{
    switch (state)
    {
        case HexagonTile.TileStates.GreenTile:
            return 3;
        case HexagonTile.TileStates.BlueTile:
            return 4;
        case HexagonTile.TileStates.RedTile:
            return 5;
        case HexagonTile.TileStates.DestroyerTile:
            return 10;
        case HexagonTile.TileStates.PakkuTile:
            return 11;
        default:
            return 0;
    }
}
    public void NextTileCreate(HexagonTile hexTile)
    { 
        
        // Apply the next tile state to the hexagon tile
      hexTile.TileStateChange(GM.futureTileStateList[0]);
      // Remove the first tile from the future tile list.
      GM.futureTileStateList.RemoveAt(0);

    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
