using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionState : States
{
    public override void Enter()
    {
        GM.changeState(GM.GetComponent<EffectState>());
        
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Fusion State");
    }
}
