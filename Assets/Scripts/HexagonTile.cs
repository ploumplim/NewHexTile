using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

using System.Linq;
using TMPro;

public class HexagonTile : MonoBehaviour
{
    public enum TileStates // These are our tiles states. To add more states, we must add its corresponding variables
    //and methods. Places where we hold this information is noted as with a ★
    {
        DefaultTile,
        GreenTile,
        BlueTile,
        RedTile,
        LegalTile,
        StarterTile,
        DeadTile,
        DestroyerTile,
        PakkuTile,
        SpreadingTile,
        // GreenFusionTile,
        // BlueFusionTile,
        // RedFusionTile,
    }
    
    public ParticleSystem explosionEffect;
    public ParticleSystem improvementEffect;
    public ParticleSystem spreaderEffect;
    public Material greenTileMaterial;
    public Material redTileMaterial;
    public Material blueTileMaterial;
    [HideInInspector] public HexagonGrid parentGrid;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public bool firstTurnCleared; //False when the tile arrives, becomes true one turn after (CounterState)
    [HideInInspector] public int lifeTime;
    [HideInInspector] public TileStates currentTileState;
    [HideInInspector] public bool canLegalize;
    
    [Tooltip("This list has all states that this tile can improve.")]
    [FormerlySerializedAs("stateToFuseWith")] 
    public List<TileStates> improvableTileStates;
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
    public int destroyerLifeTime = 1;
    public int pakkuLifeTime = 3;
    [Space]
    [HideInInspector]public int spreaderGeneration = 1;
    public int spreadingLifeTimeIncrement = 5;
    
    [Header("Improvement Values")]
    [FormerlySerializedAs("greenFusionLifeTime")] [Space]
    public int greenImproveValue = 10;
    [FormerlySerializedAs("redFusionLifeTime")]
    public int redImproveValue = 6;
    [FormerlySerializedAs("blueFusionLifeTime")]
        public int blueImproveValue = 14;
    [Header("Tile characteristics")]
    [Tooltip("How long the tile will wait before activating its effect.")]
    
    [HideInInspector]
    public GameObject currentActiveAsset;
    
    
    public void InitializeTile()
    {
        // Set the default state
        if (currentTileState!= TileStates.StarterTile)
        {
            GetComponentInChildren<TextMeshPro>().text = "";
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
                spreaderEffect.Stop();
                currentActiveAsset = tileVisuals[0];
                lifeTime = 0;
                isAlive = false;
                canLegalize = false;
                FillImprovableTiles();
                spreaderGeneration = 1;
                break;
            
            case TileStates.LegalTile:
                spreaderEffect.Stop();
                currentActiveAsset = tileVisuals[1];
                lifeTime = 0;
                isAlive = false;
                canLegalize = false;
                FillImprovableTiles();
                spreaderGeneration = 1;
                break;
            
            case TileStates.StarterTile:
                currentActiveAsset = tileVisuals[2];
                lifeTime = starterLifeTime;
                isAlive = true;
                canLegalize = true;
                FillImprovableTiles();
                LegalizeTiles();
                break;
            
            case TileStates.GreenTile:
                currentActiveAsset = tileVisuals[3];
                lifeTime = greenLifeTime;
                isAlive = true;
                canLegalize = true;
                FillImprovableTiles();
                
                break;
            
            case TileStates.BlueTile:
                currentActiveAsset = tileVisuals[4];
                lifeTime = blueLifeTime;
                isAlive = true;
                canLegalize = true;
                FillImprovableTiles();
                
                break;
            
            case TileStates.RedTile:
                currentActiveAsset = tileVisuals[5]; 
                lifeTime = redLifeTime;
                isAlive = true;
                canLegalize = true;
                FillImprovableTiles();
                
                break;
                
                //Deprecated fusion cases
            // case TileStates.GreenFusionTile:
            //     currentActiveAsset = tileVisuals[6];
            //     lifeTime += greenImproveValue;
            //     isAlive = true;
            //     FillImprovableTiles();
            //     
            //     break;
            
            // case TileStates.BlueFusionTile:
            //     currentActiveAsset = tileVisuals[7];
            //     lifeTime += blueImproveValue;
            //     isAlive = true;
            //     FillImprovableTiles();
            //     
            //     break;
            
            // case TileStates.RedFusionTile:
            //     currentActiveAsset = tileVisuals[8];
            //     lifeTime += redImproveValue;
            //     isAlive = true;
            //     FillImprovableTiles();
            
            case TileStates.DeadTile:
                spreaderEffect.Stop();
                currentActiveAsset = tileVisuals[9];
                lifeTime = 0;
                isAlive = false;
                canLegalize = false;
                spreaderGeneration = 1;
                FillImprovableTiles();
                break;
            case TileStates.DestroyerTile:
                currentActiveAsset = tileVisuals[10];
                //GetComponentInChildren<Renderer>().material.color = new Color(1f, 0f, 1f);
                lifeTime = destroyerLifeTime;
                isAlive = true;
                canLegalize = true;
                FillImprovableTiles();
                break;
            
            case TileStates.PakkuTile:
                currentActiveAsset = tileVisuals[11];
                //GetComponentInChildren<Renderer>().material.color = new Color(1f, 1f, 0f);
                lifeTime = pakkuLifeTime;
                isAlive = true;
                canLegalize = true;
                FillImprovableTiles();
                break;
            
            case TileStates.SpreadingTile:
                currentActiveAsset = tileVisuals[12];
                lifeTime = spreadingLifeTimeIncrement;
                isAlive = true;
                FillImprovableTiles();
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
    public bool HasLegalizingTiles() // returns true if it has living adjacent tiles
    {
        HexagonTile[] adjacentTiles = GetAdjacentTiles();
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (adjacentTile.canLegalize)
            {
                return true;
            }
        }
        return false;
    }
    
    private void FillImprovableTiles()
    {
        switch (currentTileState)
        {
            case TileStates.GreenTile:
            case TileStates.BlueTile:
            case TileStates.RedTile:
                
                improvableTileStates = new List<TileStates>
                {
                    TileStates.GreenTile,
                    TileStates.BlueTile,
                    TileStates.RedTile,
                    TileStates.PakkuTile,
                    TileStates.SpreadingTile,
                    TileStates.DestroyerTile
                };
                break;
            
                default:
                improvableTileStates = new List<TileStates>{};
                break;
        }
    }
    
    //Deprecated fusion codes
    // public bool CanBeFused()
    // {
    //     HexagonTile[] adjacentTiles = GetAdjacentTiles();
    //     
    //     foreach (HexagonTile adjacentTile in adjacentTiles)
    //     {
    //         if (adjacentTile != null &&
    //             stateToFuseWith.Contains(adjacentTile.currentTileState))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    //
    // public void FuseTiles()
    // {
    //     switch (currentTileState)
    //             {
    //                 case TileStates.GreenTile:
    //                     TileStateChange(TileStates.GreenFusionTile);
    //                     break;
    //                 case TileStates.RedTile:
    //                     TileStateChange(TileStates.RedFusionTile);
    //                     break;
    //                 case TileStates.BlueTile:
    //                     TileStateChange(TileStates.BlueFusionTile);
    //                     break;
    //                 default:
    //                     Debug.Log("The tile state is not recognized:" + transform);
    //                     break;
    //             }
    // }

    public void ActivateTileEffects()
    {
        switch (currentTileState)
        {
            case TileStates.BlueTile:
            case TileStates.GreenTile:
            case TileStates.RedTile:
                EffectImprove();
                break;
            
            
            case TileStates.DestroyerTile:
                EffectDestroy();
                break;
            
            case TileStates.PakkuTile:
                EffectInfect();
                break;
            
            case TileStates.SpreadingTile:
                EffectSpread();
                break;
            
            default:
                break;
        }
    }
    
    private void EffectDestroy()
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

    private void EffectInfect()
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
            // Debug.Log("Alive tiles count: " + aliveTiles.Count + ". With the highest lifetime: " + aliveTiles[0].lifeTime);
            aliveTiles[0].TileStateChange(currentTileState);
        }
        
        //If no living tiles are around, we set the tile state to default.
        else
        {
            TileStateChange(TileStates.DefaultTile);
        }
    }

    private void EffectImprove()
    {
        // If the tile has already cleared its first turn, we return to avoid triggering the effect again.
        if (firstTurnCleared)
        {
            return;
        }
        
        // We make a list of all tiles around this one.
        List<HexagonTile> improvableAdjacentTiles = GetAdjacentTiles().ToList();
        
        // We iterate through the list and check if the tile is improvable. If not, we remove it from the list.
        foreach (HexagonTile adjacentTile in GetAdjacentTiles())
        {
            if (!improvableTileStates.Contains(adjacentTile.currentTileState))
            {
                improvableAdjacentTiles.Remove(adjacentTile);
            }
            
        }
       
        //TODO Faire l'afficgage des effets d'amélioration
        // We iterate through the list and improve the tiles depending on my current tile's state.
        foreach (HexagonTile tile in improvableAdjacentTiles)
        {
            switch (currentTileState)
            {
                case TileStates.GreenTile:
                    tile.improvementEffect.GetComponent<Renderer>().material = greenTileMaterial;
                    tile.improvementEffect.Play();
                    tile.lifeTime += greenImproveValue;
                    break;
                case TileStates.BlueTile:
                    tile.improvementEffect.GetComponent<Renderer>().material = blueTileMaterial;
                    tile.improvementEffect.Play();
                    tile.lifeTime += blueImproveValue;
                    break;
                case TileStates.RedTile:
                    tile.improvementEffect.GetComponent<Renderer>().material = redTileMaterial;
                    tile.improvementEffect.Play();
                    tile.lifeTime += redImproveValue;
                    break;
                default:
                    Debug.Log("Invalid tile with the improve effect :" + transform);
                    break;
            }
        }
    }
    
   
    private void EffectSpread()
    {
        var emission = spreaderEffect.emission;
        switch (lifeTime)
                {
                    case > 3:
                        spreaderEffect.Stop();
                        break;
                    case 3:
                        spreaderEffect.Play();
                        emission.rateOverTime = 6;
                        break;
                    case 2:
                        spreaderEffect.Play();
                        emission.rateOverTime = 12;
                        break;
                    case 1:
                        spreaderEffect.Stop();
                        break;
                }
        if (lifeTime != 1)
        {
            return;
        }

        
        
        lifeTime += spreadingLifeTimeIncrement * spreaderGeneration;
        
        
        // We make a list of all tiles around this one.
        HexagonTile[] adjacentTiles = GetAdjacentTiles();

        // We iterate through the list and spread the effect to the tiles around this one.
        foreach (HexagonTile adjacentTile in adjacentTiles)
        {
            if (!adjacentTile.isAlive && adjacentTile.currentTileState != TileStates.DeadTile)
            {
                adjacentTile.TileStateChange(currentTileState);
                adjacentTile.spreaderGeneration = spreaderGeneration + 1;
                adjacentTile.lifeTime += spreadingLifeTimeIncrement * adjacentTile.spreaderGeneration - spreadingLifeTimeIncrement;
            }
        }
    }
}