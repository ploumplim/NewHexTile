using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public States currentState;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        States[] states = GetComponents<States>();
        foreach (States state in states)
        {
            state.Initialize(this);
        }
        DontDestroyOnLoad(gameObject);
       
        // currentState = GetComponent<UpkeepState>();
        // currentState.Enter();
    }
    
    private void Update()
    {
        currentState?.Tick();
    }
    
}
