using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class HexagonTile : MonoBehaviour
{
    [HideInInspector]
    public enum TileStates // These are our tiles states. To add more states, we must add its corresponding variables
    //and methods. Places where we hold this information is noted as with a ★
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
        PakkuTile
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
    
    // Tile life times editable in inspector ★
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
    public int pakkuLifeTime = 3;
    [Space]
    
    [Header("Tile characteristics")]
    [Tooltip("How long the tile will wait before activating its effect.")]
    
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
        
        // Change the tile's current state variable. ★
        // Implement behavior modification based on the state
        switch (state)
        {
            case TileStates.DefaultTile:
                currentActiveAsset = tileVisuals[0];
                lifeTime = 0;
                isAlive = false;
                FillStatesToFuseWith();
                break;
            
            case TileStates.LegalTile:
                currentActiveAsset = tileVisuals[1];
                lifeTime = 0;
                isAlive = false;
                FillStatesToFuseWith();
                break;
            
            case TileStates.StarterTile:
                currentActiveAsset = tileVisuals[2];
                lifeTime = starterLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                LegalizeTiles();
                break;
            
            case TileStates.GreenTile:
                currentActiveAsset = tileVisuals[3];
                lifeTime = greenLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.BlueTile:
                currentActiveAsset = tileVisuals[4];
                lifeTime = blueLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.RedTile:
                currentActiveAsset = tileVisuals[5]; 
                lifeTime = redLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.GreenFusionTile:
                currentActiveAsset = tileVisuals[6];
                lifeTime += greenFusionLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.BlueFusionTile:
                currentActiveAsset = tileVisuals[7];
                lifeTime += blueFusionLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            
            case TileStates.RedFusionTile:
                currentActiveAsset = tileVisuals[8];
                lifeTime += redFusionLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                
                break;
            case TileStates.DeadTile:
                currentActiveAsset = tileVisuals[9];
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
            
            case TileStates.PakkuTile:
                currentActiveAsset = tileVisuals[11];
                //GetComponentInChildren<Renderer>().material.color = new Color(1f, 1f, 0f);
                lifeTime = pakkuLifeTime;
                isAlive = true;
                FillStatesToFuseWith();
                break;
                
                
            default:
                Debug.Log("The tile state is not recognized:" + transform);
                break;
            
        }
        
        // Set the current active asset to the current tile state
        if (currentActiveAsset != null)
        {
            //previewLister[0].SetActive(false);
            currentActiveAsset.SetActive(true);
            foreach (var asset in tileVisuals)
            {
                if (asset != currentActiveAsset)
                { // If the asset is not the current active asset, we want to deactivate it.
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
            
            case TileStates.PakkuTile:
                EffectInfect();
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

    public void EffectInfect()
    {
        //Debug.Log("tile at hexagrid position:"+ transform.position +"PakkuCounter: " + PakkuCounter);
        if (lifeTime != 1)
        {
            return;
        }
        // we make a list of all tiles around this one.
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        
        // we check if any of them are alive and add them to an aliveTiles list.
        List<HexagonTile> aliveTiles = new List<HexagonTile>();
        
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.isAlive)
            {
                aliveTiles.Add(adjacentTile);
            }
        }
        
        // Then, we sort the list by their lifetime, with the longest life time being the first element.
        aliveTiles.Sort((x, y) => y.lifeTime.CompareTo(x.lifeTime));
        // We then infect the first element in the list.
        if (aliveTiles.Count > 0)
        { 
            Debug.Log("Alive tiles count: " + aliveTiles.Count + ". With the highest lifetime: " + aliveTiles[0].lifeTime);
            aliveTiles[0].TileStateChange(currentTileState);
        }
        
        //If no living tiles are around, we set the tile state to default.
        else
        {
            TileStateChange(TileStates.DefaultTile);
        }
    }
    
}