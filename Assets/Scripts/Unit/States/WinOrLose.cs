public class WinOrLose : State<UnitData>
{
    public WinOrLose(UnitData data, StateMachine<UnitData> stateMachine) : base(data, stateMachine) { }
    public override void LogicUpdate()
    {
        if(GameState.Instance.IsWinOrLose)Data.PlayWinOrLoseAnimation(GameState.Instance.IsWin);
    }
}
