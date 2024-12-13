using System;
using System.Collections.Generic;
using HexStates;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class HexagonTile : MonoBehaviour
{
    [HideInInspector]
    public enum TileStates // These are our tiles states
    {
        DefaultTile,
        GreenTile,
        BlueTile,
        RedTile,
        LegalTile,
        StarterTile,
        GreenFusionTile,
        BlueFusionTile,
        RedFusionTile,
        DeadTile,
        DestroyerTile,
    }
    [HideInInspector]
    public HexagonGrid parentGrid;
    [HideInInspector]
    public bool isAlive;
    [HideInInspector]
    public int lifeTime;
    [HideInInspector]
    public List<TileStates> stateToFuseWith;
    [HideInInspector]
    public TileStates currentTileState;
    
    // Tile life times editable in inspector
    public int starterLifeTime = 1;
    
    public int greenLifeTime = 5;
    public int redLifeTime = 3;
    public int blueLifeTime = 7;
    public int destroyerLifeTime = 1;
    
    public int greenFusionLifeTime = 10;
    public int redFusionLifeTime = 6;
    public int blueFusionLifeTime = 14;
    
    
    
    
    public void InitializeTile()
    {
        // Set the default state
        if (currentTileState!= TileStates.StarterTile)
        {
            currentTileState = TileStates.DefaultTile;
            TileStateChange(TileStates.DefaultTile);
        }
    }
    
    public void TileStateChange(TileStates state)
    {
        currentTileState = state;
        // Change the tile's current state variable.
        // Implement behavior modification based on the state
        switch (state)
        {
            case TileStates.DefaultTile:
                GetComponentInChildren<Renderer>().material.color = Color.grey;
                Empty defaultTile = GetComponent<Empty>();
                if (defaultTile == null)
                {
                    defaultTile = gameObject.AddComponent<Empty>();
                }
                defaultTile.Init();
                break;
            
            case TileStates.LegalTile:
                GetComponentInChildren<Renderer>().material.color = Color.white;
                Legal legalTile = GetComponent<Legal>();
                if (legalTile == null)
                {
                    legalTile = gameObject.AddComponent<Legal>();
                }
                legalTile.Init();
                break;
            
            case TileStates.StarterTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f,0f,0f);
                StarterTile starterTile = GetComponent<StarterTile>();
                if (starterTile == null)
                {
                    starterTile = gameObject.AddComponent<StarterTile>();
                }
                starterTile.Init();
                break;
            
            case TileStates.GreenTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f,0.5f,0f);
                Tile_basic basicTile = GetComponent<Tile_basic>();
                if (basicTile == null)
                {
                    basicTile = gameObject.AddComponent<Tile_basic>();
                }
                basicTile.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.BlueTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f,0f,0.5f);
                Tile_Slow slowTile = GetComponent<Tile_Slow>();
                if (slowTile == null)
                {
                    slowTile = gameObject.AddComponent<Tile_Slow>();
                }
                slowTile.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.RedTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0.5f,0f,0f);
                Tile_Fast fastTile = GetComponent<Tile_Fast>();
                if (fastTile == null)
                {
                    fastTile = gameObject.AddComponent<Tile_Fast>();
                }
                fastTile.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.GreenFusionTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f,1f,0f);
                Fusion1 fusion1 = GetComponent<Fusion1>();
                if (fusion1 == null)
                {
                    fusion1 = gameObject.AddComponent<Fusion1>();
                }
                fusion1.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.BlueFusionTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f, 0f, 1f);
                Fusion_slow fusionSlow = GetComponent<Fusion_slow>();
                if (fusionSlow == null)
                {
                    fusionSlow = gameObject.AddComponent<Fusion_slow>();
                }
                fusionSlow.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.RedFusionTile:
                GetComponentInChildren<Renderer>().material.color = new Color(1f, 0f, 0f);
                Fusion_fast fusionFast = GetComponent<Fusion_fast>();
                if (fusionFast == null)
                {
                    fusionFast = gameObject.AddComponent<Fusion_fast>();
                }
                fusionFast.Init();
                FillStatesToFuseWith();
                
                break;
            case TileStates.DeadTile:
                GetComponentInChildren<Renderer>().material.color = Color.black;
                Dead_state deadState = GetComponent<Dead_state>();
                if (deadState == null)
                {
                    deadState = gameObject.AddComponent<Dead_state>();
                }
                deadState.Init();
                FillStatesToFuseWith();

                break;
            case TileStates.DestroyerTile:
                GetComponentInChildren<Renderer>().material.color = new Color(1f, 0f, 1f);
                DestroyerTile destroyerTile = GetComponent<DestroyerTile>();
                if (destroyerTile == null)
                {
                    destroyerTile = gameObject.AddComponent<DestroyerTile>();
                }
                destroyerTile.Init();
                FillStatesToFuseWith();
                break;
                
                
            default:
                Debug.Log("The tile state is not recognized:" + transform);
                GetComponentInChildren<Renderer>().material.color = Color.black;
                break;
            
        }
    }

    public HexagonTile[] GetAdjacentTiles()
    {
        List<HexagonTile> adjacentTiles = new List<HexagonTile>();
        Vector3[] directions = new Vector3[]
        {
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3), 0, 0),                                // Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3), 0, 0),                               // Left
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, parentGrid.tileScale * 1.5f),  // Up-Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, parentGrid.tileScale * 1.5f), // Up-Left
            new Vector3(parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, -parentGrid.tileScale * 1.5f), // Down-Right
            new Vector3(-parentGrid.tileScale * Mathf.Sqrt(3) / 2, 0, -parentGrid.tileScale * 1.5f) // Down-Left
        };

        foreach (Vector3 direction in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, parentGrid.tileScale * 2))
            {
                adjacentTiles.Add(hit.collider.gameObject.GetComponent<HexagonTile>());
            }
        }

        return adjacentTiles.ToArray();
    }

    public void LegalizeTiles()
    {
        HexagonTile hexTile = GetComponent<HexagonTile>();
        if (hexTile != null)
        {
            HexagonTile[] adjacentTiles = hexTile.GetAdjacentTiles();
            
            foreach (HexagonTile adjacentTile in adjacentTiles)
            {
                if (adjacentTile.currentTileState == TileStates.DefaultTile)
                {
                    adjacentTile.TileStateChange(TileStates.LegalTile);
                }
            }
        }
    }

    public void EndOfLifeTime()
    {
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        
        // Kill my cell
        TileStateChange(TileStates.DeadTile);
                
        // Check if the adjacent cells should remain legal        
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.currentTileState == TileStates.LegalTile && LegalTileShouldBeDefault(adjacentTile))
            {
                adjacentTile.TileStateChange(TileStates.DefaultTile);
            }
        }
    }
    
    public bool LegalTileShouldBeDefault(HexagonTile hexTile) // This method should only be called by dying cells to have any legal tiles around them check if they should still be legal.
    {
        HexagonTile[] adjacentTiles = hexTile.GetAdjacentTiles();
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.currentTileState != TileStates.LegalTile &&
                adjacentTile.currentTileState != TileStates.DefaultTile &&
                adjacentTile.currentTileState != TileStates.DeadTile)
            {
                return false;
            }
        }
        return true;
    }
    
    public void FillStatesToFuseWith()
    {
        switch (currentTileState)
        {
            case TileStates.GreenTile:
                stateToFuseWith = new List<TileStates>
                {
                    TileStates.GreenTile
                };
                break;
            case TileStates.RedTile:
                stateToFuseWith = new List<TileStates>
                {
                    TileStates.RedTile
                };
                break;
            case TileStates.BlueTile:
                stateToFuseWith = new List<TileStates>
                {
                    TileStates.BlueTile
                };
                break;
            default:
                stateToFuseWith = new List<TileStates>{};
                break;
        }
    }


    public bool CanBeFused()
    {
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile != null &&
                stateToFuseWith.Contains(adjacentTile.currentTileState))
            {
                return true;
            }
        }
        return false;
    }
    public void FuseTiles()
    {
        switch (currentTileState)
                {
                    case TileStates.GreenTile:
                        TileStateChange(TileStates.GreenFusionTile);
                        break;
                    case TileStates.RedTile:
                        TileStateChange(TileStates.RedFusionTile);
                        break;
                    case TileStates.BlueTile:
                        TileStateChange(TileStates.BlueFusionTile);
                        break;
                    default:
                        Debug.Log("The tile state is not recognized:" + transform);
                        break;
                }
    }

    public void ActivateTileEffects()
    {
        switch (currentTileState)
        {
            case TileStates.DestroyerTile:
                EffectDestroy();
                break;
            
            default:
                break;
        }
    }
    
    public void EffectDestroy()
    {
        EndOfLifeTime();
        TileStateChange(TileStates.DefaultTile);
        
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.isAlive)
            {
                adjacentTile.EndOfLifeTime();

                if (adjacentTile.currentTileState == TileStates.DeadTile)
                {
                    adjacentTile.TileStateChange(TileStates.LegalTile);
                    
                    if (adjacentTile.LegalTileShouldBeDefault(adjacentTile))
                    {
                        adjacentTile.TileStateChange(TileStates.DefaultTile);
                    }
                }

            }

            if (!adjacentTile.isAlive)
            {
                if (adjacentTile.currentTileState == TileStates.DeadTile)
                {
                    adjacentTile.TileStateChange(TileStates.LegalTile);

                    if (adjacentTile.LegalTileShouldBeDefault(adjacentTile))
                    {
                        adjacentTile.TileStateChange(TileStates.DefaultTile);
                    }
                }
            }
        }
    }

    public void EffectRot()
    {
        
    }
    
}