using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectState : States
{
    public override void Enter()
    {
        GM.changeState(GM.GetComponent<CountersState>());
    }

    public override void Tick()
    {
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Effect State");
    }
}
