using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiChanger : MonoBehaviour
{
    
    public static  void ChangeTextColor( TextMeshPro textMesh, Color color)
    {
        textMesh.color = color;
    }
}
