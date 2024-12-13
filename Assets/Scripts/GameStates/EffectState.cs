using UnityEngine;

public class EffectState : States
{
    public override void Enter()
    {
        // For each hexagon tile in our livingTiles list, we want to activate the tile effects
        foreach (HexagonTile tile in GM.livingTiles)
        {
            tile.GetComponent<HexagonTile>().ActivateTileEffects();
        }
        
        
        GM.ChangeState(GM.GetComponent<CountersState>());
    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Effect State");
    }
}
