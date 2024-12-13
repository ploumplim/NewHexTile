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
    public HexagonGrid parentGrid;
    [HideInInspector]
    public TileState tileState;
    [HideInInspector]
    public bool isAlive;
    [HideInInspector]
    public int lifeTime;
    [HideInInspector]
    public List<TileState.TileStates> stateToFuseWith;
    
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
        tileState = GetComponent<TileState>();
        tileState.init();
    }
    
    public void TileStateChange(TileState.TileStates state)
    {
        // Implement behavior modification based on the state
        switch (state)
        {
            case TileState.TileStates.DefaultTile:
                GetComponentInChildren<Renderer>().material.color = Color.grey;
                Empty defaultTile = GetComponent<Empty>();
                if (defaultTile == null)
                {
                    defaultTile = gameObject.AddComponent<Empty>();
                }
                defaultTile.Init();
                break;
            
            case TileState.TileStates.LegalTile:
                GetComponentInChildren<Renderer>().material.color = Color.white;
                Legal legalTile = GetComponent<Legal>();
                if (legalTile == null)
                {
                    legalTile = gameObject.AddComponent<Legal>();
                }
                legalTile.Init();
                break;
            
            case TileState.TileStates.StarterTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f,0f,0f);
                StarterTile starterTile = GetComponent<StarterTile>();
                if (starterTile == null)
                {
                    starterTile = gameObject.AddComponent<StarterTile>();
                }
                starterTile.Init();
                break;
            
            case TileState.TileStates.GreenTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f,0.5f,0f);
                Tile_basic basicTile = GetComponent<Tile_basic>();
                if (basicTile == null)
                {
                    basicTile = gameObject.AddComponent<Tile_basic>();
                }
                basicTile.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileState.TileStates.BlueTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f,0f,0.5f);
                Tile_Slow slowTile = GetComponent<Tile_Slow>();
                if (slowTile == null)
                {
                    slowTile = gameObject.AddComponent<Tile_Slow>();
                }
                slowTile.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileState.TileStates.RedTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0.5f,0f,0f);
                Tile_Fast fastTile = GetComponent<Tile_Fast>();
                if (fastTile == null)
                {
                    fastTile = gameObject.AddComponent<Tile_Fast>();
                }
                fastTile.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileState.TileStates.GreenFusionTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f,1f,0f);
                Fusion1 fusion1 = GetComponent<Fusion1>();
                if (fusion1 == null)
                {
                    fusion1 = gameObject.AddComponent<Fusion1>();
                }
                fusion1.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileState.TileStates.BlueFusionTile:
                GetComponentInChildren<Renderer>().material.color = new Color(0f, 0f, 1f);
                Fusion_slow fusionSlow = GetComponent<Fusion_slow>();
                if (fusionSlow == null)
                {
                    fusionSlow = gameObject.AddComponent<Fusion_slow>();
                }
                fusionSlow.Init();
                FillStatesToFuseWith();
                
                break;
            
            case TileState.TileStates.RedFusionTile:
                GetComponentInChildren<Renderer>().material.color = new Color(1f, 0f, 0f);
                Fusion_fast fusionFast = GetComponent<Fusion_fast>();
                if (fusionFast == null)
                {
                    fusionFast = gameObject.AddComponent<Fusion_fast>();
                }
                fusionFast.Init();
                FillStatesToFuseWith();
                
                break;
            case TileState.TileStates.DeadTile:
                GetComponentInChildren<Renderer>().material.color = Color.black;
                Dead_state deadState = GetComponent<Dead_state>();
                if (deadState == null)
                {
                    deadState = gameObject.AddComponent<Dead_state>();
                }
                deadState.Init();
                FillStatesToFuseWith();

                break;
            case TileState.TileStates.DestroyerTile:
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
                if (adjacentTile.tileState.currentState == TileState.TileStates.DefaultTile)
                {
                    adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.LegalTile);
                }
            }
        }
    }

    public void EndOfLifeTime()
    {
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        
        // Kill my cell
        tileState.ApplyState(this, TileState.TileStates.DeadTile);
                
        // Check if the adjacent cells should remain legal        
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.tileState.currentState == TileState.TileStates.LegalTile && LegalTileShouldBeDefault(adjacentTile))
            {
                adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.DefaultTile);
            }
        }
    }
    
    public bool LegalTileShouldBeDefault(HexagonTile hexTile) // This method should only be called by dying cells to have any legal tiles around them check if they should still be legal.
    {
        HexagonTile[] adjacentTiles = hexTile.GetAdjacentTiles();
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.tileState.currentState != TileState.TileStates.LegalTile &&
                adjacentTile.tileState.currentState != TileState.TileStates.DefaultTile &&
                adjacentTile.tileState.currentState != TileState.TileStates.DeadTile)
            {
                return false;
            }
        }
        return true;
    }
    
    public void FillStatesToFuseWith()
    {
        switch (tileState.currentState)
        {
            case TileState.TileStates.GreenTile:
                stateToFuseWith = new List<TileState.TileStates>
                {
                    TileState.TileStates.GreenTile
                };
                break;
            case TileState.TileStates.RedTile:
                stateToFuseWith = new List<TileState.TileStates>
                {
                    TileState.TileStates.RedTile
                };
                break;
            case TileState.TileStates.BlueTile:
                stateToFuseWith = new List<TileState.TileStates>
                {
                    TileState.TileStates.BlueTile
                };
                break;
            default:
                stateToFuseWith = new List<TileState.TileStates>{};
                break;
        }
    }


    public bool CanBeFused()
    {
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile != null &&
                adjacentTile.tileState != null &&
                stateToFuseWith.Contains(adjacentTile.tileState.currentState))
            {
                return true;
            }
        }
        return false;
    }
    public void FuseTiles()
    {
        switch (tileState.currentState)
                {
                    case TileState.TileStates.GreenTile:
                        tileState.ApplyState(this, TileState.TileStates.GreenFusionTile);
                        break;
                    case TileState.TileStates.RedTile:
                        tileState.ApplyState(this, TileState.TileStates.RedFusionTile);
                        break;
                    case TileState.TileStates.BlueTile:
                        tileState.ApplyState(this, TileState.TileStates.BlueFusionTile);
                        break;
                    default:
                        Debug.Log("The tile state is not recognized:" + transform);
                        break;
                }
    }

    public void ActivateTileEffects()
    {
        var state = tileState.currentState;
        switch (state)
        {
            case TileState.TileStates.DestroyerTile:
                EffectDestroy();
                break;
            
            default:
                break;
        }
    }
    
    public void EffectDestroy()
    {
        EndOfLifeTime();
        tileState.ApplyState(this, TileState.TileStates.DefaultTile);
        
        
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.isAlive)
            {
                adjacentTile.EndOfLifeTime();

                if (adjacentTile.tileState.currentState == TileState.TileStates.DeadTile)
                {
                    adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.LegalTile);
                    
                    if (adjacentTile.LegalTileShouldBeDefault(adjacentTile))
                    {
                        adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.DefaultTile);
                    }
                }

            }

            if (!adjacentTile.isAlive)
            {
                if (adjacentTile.tileState.currentState == TileState.TileStates.DeadTile)
                {
                    adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.LegalTile);

                    if (adjacentTile.LegalTileShouldBeDefault(adjacentTile))
                    {
                        adjacentTile.tileState.ApplyState(adjacentTile, TileState.TileStates.DefaultTile);
                    }
                }
            }
        }
    }

    public void EffectRot()
    {
        
    }
    
}