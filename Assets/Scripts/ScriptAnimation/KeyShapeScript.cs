using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyShapeScript : MonoBehaviour
{
    public HexagonTile hexagonTile;
    private float keyShape;
    private SkinnedMeshRenderer sMR;

    private void Awake()
    {
      sMR = GetComponent<SkinnedMeshRenderer>();  
    }

    private void Update()
    {
        keyShape = 500/hexagonTile.lifeTime;

        if (keyShape >= 100)
        {
            sMR.SetBlendShapeWeight(0, 100);
        }
        else {
            sMR.SetBlendShapeWeight(0, keyShape);
                }
        }
}
