using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationScrpt : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer;
    public HexagonTile HexagonTile;
    // Start is called before the first frame update
    private void Awake()
    {
        

        
    }
    public void Update()
    {
        if (HexagonTile.lifeTime >=15)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 10);
        }

        if (HexagonTile.lifeTime == 10)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 30);
        }
        if (HexagonTile.lifeTime == 8)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 45);
        }
        if (HexagonTile.lifeTime == 6)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 66);
        }
        if (HexagonTile.lifeTime == 5)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 70);
        }
        if (HexagonTile.lifeTime == 4)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 80);
        }
        if (HexagonTile.lifeTime == 3)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 90);
        }
        if (HexagonTile.lifeTime == 2)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 95);
        }
        if (HexagonTile.lifeTime == 1)
        {
            SkinnedMeshRenderer.SetBlendShapeWeight(0, 98);
        }

    }
}
