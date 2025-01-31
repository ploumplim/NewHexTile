using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationScrpt : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer;
    HexagonTile HexagonTile;
    // Start is called before the first frame update
    private void Awake()
    {
        HexagonTile = FindAnyObjectByType<HexagonTile>();

        SkinnedMeshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(0, 100, HexagonTile.lifeTime));
    }
}
