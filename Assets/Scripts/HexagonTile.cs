using System;
using System.Collections.Generic;
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
        DestroyerTile
    }

    public ParticleSystem explosionEffect;
    [HideInInspector]
    public HexagonGrid parentGrid;
    [HideInInspector]
    public bool isAlive;
    [HideInInspector]
    public bool doesntLegalize;
    [HideInInspector]
    public int lifeTime;
    [HideInInspector]
    public List<TileStates> stateToFuseWith;
    [HideInInspector]
    public TileStates currentTileState;
    
    [Tooltip("Drag and drop new visuals for the tiles here.")] 
    [FormerlySerializedAs("previewLister")] public List<GameObject> tileVisuals;
    
    // Tile life times editable in inspector
    [Header("Tile Life Times")]
    public int starterLifeTime = 1;
    [Space]
    public int greenLifeTime = 5;
    public int redLifeTime = 3;
    public int blueLifeTime = 7;
    [Space]
    public int greenFusionLifeTime = 10;
    public int redFusionLifeTime = 6;
    public int blueFusionLifeTime = 14;
    [Space]
    public int destroyerLifeTime = 1;
    [Space]
    
    [HideInInspector]
    public GameObject currentActiveAsset;
    
    
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
                currentActiveAsset = tileVisuals[0];
                //GetComponentInChildren<Renderer>().material.color = Color.grey;
                lifeTime = 0;
                isAlive = false;
                FillStatesToFuseWith();
                break;
            
            case TileStates.LegalTile:
                currentActiveAsset = tileVisuals[1];
                //GetComponentInChildren<Renderer>().material.color = Color.white;
                lifeTime = 0;
                isAlive = false;
                FillStatesToFuseWith();
                break;
            
            case TileStates.StarterTile:
                currentActiveAsset = tileVisuals[2];
                //GetComponentInChildren<Renderer>().material.color = new Color(0f,0f,0f);
                lifeTime = starterLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                LegalizeTiles();
                break;
            
            case TileStates.GreenTile:
                currentActiveAsset = tileVisuals[3];
                //GetComponentInChildren<Renderer>().material.color = new Color(0f,0.5f,0f);
                lifeTime = greenLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.BlueTile:
                currentActiveAsset = tileVisuals[4];
                //GetComponentInChildren<Renderer>().material.color = new Color(0f,0f,0.5f);
                lifeTime = blueLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.RedTile:
                currentActiveAsset = tileVisuals[5];
                //GetComponentInChildren<Renderer>().material.color = new Color(0.5f,0f,0f);
                lifeTime = redLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.GreenFusionTile:
                currentActiveAsset = tileVisuals[6];
                GetComponentInChildren<Renderer>().material.color = new Color(0f,1f,0f);
                lifeTime += greenFusionLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.BlueFusionTile:
                currentActiveAsset = tileVisuals[7];
                GetComponentInChildren<Renderer>().material.color = new Color(0f, 0f, 1f);
                lifeTime += blueFusionLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.RedFusionTile:
                currentActiveAsset = tileVisuals[8];
                GetComponentInChildren<Renderer>().material.color = new Color(1f, 0f, 0f);
                lifeTime += redFusionLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            case TileStates.DeadTile:
                currentActiveAsset = tileVisuals[9];
                GetComponentInChildren<Renderer>().material.color = Color.black;
                lifeTime = 0;
                isAlive = false;
                FillStatesToFuseWith();
                break;
            case TileStates.DestroyerTile:
                currentActiveAsset = tileVisuals[10];
                //GetComponentInChildren<Renderer>().material.color = new Color(1f, 0f, 1f);
                lifeTime = destroyerLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                break;
                
                
            default:
                Debug.Log("The tile state is not recognized:" + transform);
                GetComponentInChildren<Renderer>().material.color = Color.black;
                break;
            
        }
        if (currentActiveAsset != null)
        {
            //previewLister[0].SetActive(false);
            currentActiveAsset.SetActive(true);
            foreach (var asset in tileVisuals)
            {
                if (asset != currentActiveAsset)
                {
                    asset.SetActive(false);
                }
            }
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
        
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
            
            foreach (HexagonTile adjacentTile in adjacentTiles)
            {
                if (adjacentTile.currentTileState == TileStates.DefaultTile)
                {
                    adjacentTile.TileStateChange(TileStates.LegalTile);
                }
            }
    }
    public bool HasLivingAdjacentTiles() // returns true if has living adjacent tiles
    {
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.isAlive)
            {
                return true;
            }
        }
        return false;
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
        
       
        if (lifeTime == 1)
        {

            // activate the explosion effect
            explosionEffect.Play();
            // we set its state to default.
            TileStateChange(TileStates.DefaultTile);
            // we make a list of all tiles around this one.
            HexagonTile[] adjacentTiles = GetAdjacentTiles();
            // we iterate through the list and activate the explosion effect on each tile, then set their state to default.
            foreach (HexagonTile adjacentTile in adjacentTiles)
            {
                if (adjacentTile.isAlive || adjacentTile.currentTileState == TileStates.DeadTile)
                {
                    adjacentTile.explosionEffect.Play();
                }
                adjacentTile.TileStateChange(TileStates.DefaultTile);
            }
        }
    }

    public void EffectRot()
    {
        
    }
    
}