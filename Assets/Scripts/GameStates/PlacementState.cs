using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacementState : States
{
    public override void Enter()
    {
        // Generate a preview of the next tiles that the player can choose.
        switch (GM.nextTile1)
        {
            case 0: //green tile
                GM.nextTilePreview1.GetComponentInChildren<Image>().color = GM.greenTileColor;
                GM.nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + GM.greenLifeTime + ",+" + GM.greenFusionLifeTime;
                break;
            case 1: //blue tile
                GM.nextTilePreview1.GetComponentInChildren<Image>().color = GM.blueTileColor;
                GM.nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + GM.blueLifeTime + ",+" + GM.blueFusionLifeTime;
                break;
            case 2: //red tile
                GM.nextTilePreview1.GetComponentInChildren<Image>().color = GM.redTileColor;
                GM.nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + GM.redLifeTime + ",+" + GM.redFusionLifeTime;
                break;
            case 3: //destroyer tile
                GM.nextTilePreview1.GetComponentInChildren<Image>().color = GM.destroyerTileColor;
                GM.nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = GM.destroyerText;
                break;
            case 4: //Pakku tile
                GM.nextTilePreview1.GetComponentInChildren<Image>().color = GM.pakkuTileColor;
                GM.nextTilePreview1.GetComponentInChildren<TextMeshProUGUI>().text = GM.pakkuText + "LT:" + GM.pakkuLifeTime;
                break;
        }
        
        switch (GM.nextTile2)
        {
            case 0: //green tile
                GM.nextTilePreview2.GetComponentInChildren<Image>().color = GM.greenTileColor;
                GM.nextTilePreview2.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + GM.greenLifeTime + ",+" + GM.greenFusionLifeTime;
                break;
            case 1: //blue tile
                GM.nextTilePreview2.GetComponentInChildren<Image>().color = GM.blueTileColor;
                GM.nextTilePreview2.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + GM.blueLifeTime + ",+" + GM.blueFusionLifeTime;
                break;
            case 2: //red tile
                GM.nextTilePreview2.GetComponentInChildren<Image>().color = GM.redTileColor;
                GM.nextTilePreview2.GetComponentInChildren<TextMeshProUGUI>().text = "LT: " + GM.redLifeTime + ",+" + GM.redFusionLifeTime;
                break;
            case 3: //destroyer tile
                GM.nextTilePreview2.GetComponentInChildren<Image>().color = GM.destroyerTileColor;
                GM.nextTilePreview2.GetComponentInChildren<TextMeshProUGUI>().text = GM.destroyerText;
                break;
            case 4: //Pakku tile
                GM.nextTilePreview2.GetComponentInChildren<Image>().color = GM.pakkuTileColor;
                GM.nextTilePreview2.GetComponentInChildren<TextMeshProUGUI>().text = GM.pakkuText + "LT:" + GM.pakkuLifeTime;
                break;
        }
    }
    
    public override void Tick()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                
                // If the ray hits a hexagon tile, we want to apply the next tile state to it.
                HexagonTile hexTile = hit.collider.GetComponent<HexagonTile>();
                if (hexTile != null)
                {
                    if (hexTile.currentTileState == HexagonTile.TileStates.LegalTile)
                    {
                        // If godmode is enabled, we want to apply the tile state that is selected in the godHUD
                        if (GM.GODMODE)
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
                        else // If godmode is disabled, we want to apply the next tile state to the hexagon tile
                        {
                            NextTileCreate(hexTile);
                        }
                        
                        
                        GM.ChangeState(GM.GetComponent<EffectState>());

                        
                    }
                }
            }
        }
    }
    
   
    
    public void NextTileCreate(HexagonTile hexTile)
    {
        // Apply the next tile state to the hexagon tile
        int playerTileChoice = GM.nextTileSelectorToggle.activeTile;

        if (playerTileChoice == 0)
        {
            hexTile.currentActiveAsset = hexTile.tileVisuals[GM.nextTile1];
            switch (GM.nextTile1)
            {
                case 0:
                    hexTile.TileStateChange(HexagonTile.TileStates.GreenTile);
                    break;
                case 1:
                    hexTile.TileStateChange(HexagonTile.TileStates.BlueTile);
                    break;
                case 2:
                    hexTile.TileStateChange(HexagonTile.TileStates.RedTile);
                    break;
                case 3:
                    hexTile.TileStateChange(HexagonTile.TileStates.DestroyerTile);
                    break;
                case 4:
                    hexTile.TileStateChange(HexagonTile.TileStates.PakkuTile);
                    break;
            }
        }

        if (playerTileChoice == 1)
        {
            hexTile.currentActiveAsset = hexTile.tileVisuals[GM.nextTile2];
            switch (GM.nextTile2)
            {
                case 0:
                    hexTile.TileStateChange(HexagonTile.TileStates.GreenTile);
                    break;
                case 1:
                    hexTile.TileStateChange(HexagonTile.TileStates.BlueTile);
                    break;
                case 2:
                    hexTile.TileStateChange(HexagonTile.TileStates.RedTile);
                    break;
                case 3:
                    hexTile.TileStateChange(HexagonTile.TileStates.DestroyerTile);
                    break;
                case 4:
                    hexTile.TileStateChange(HexagonTile.TileStates.PakkuTile);
                    break;
            }
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
