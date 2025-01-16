using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Events")]
    [field: SerializeField] public EventReference bombaSound { get; private set; }
    [field: SerializeField] public EventReference clickSound { get; private set; }
    [field: SerializeField] public EventReference hoverSound { get; private set; }
    [field: SerializeField] public EventReference improveSound { get; private set; }





    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one FMODEvents in the scene");
        }
        instance = this;
    }   
}
