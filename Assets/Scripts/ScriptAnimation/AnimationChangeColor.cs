using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChangeColor : MonoBehaviour
{
    public GameObject objChange; 
    public HexagonTile hexagonTile;
    public Material initMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hexagonTile.lifeTime == 1)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.color = Color.grey;
            objChange.GetComponent<MeshRenderer>().material = material;
        }
        if (hexagonTile.lifeTime != 1)
        {
            objChange.GetComponent<MeshRenderer>().material = initMat;
        }
    }
}
