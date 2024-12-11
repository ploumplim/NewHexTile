public class EffectState : States
{
    public override void Enter()
    {
        
        foreach (HexagonTile tile in GM.livingTiles)
        {
            tile.GetComponent<HexagonTile>().ActivateTileEffects();
        }
        
        
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
