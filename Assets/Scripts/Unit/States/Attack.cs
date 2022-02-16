public class Attack : State<UnitData>
{
    public Attack(UnitData data, StateMachine<UnitData> stateMachine) : base(data, stateMachine) { }
    public override void Enter()
    {
        Data.AttackEnemy();
    }
    public override void LogicUpdate()
    {
        if (GameState.Instance.IsWinOrLose)
        {
            Data.PlayWinOrLoseAnimation(GameState.Instance.IsWin);
            Machine.ChangeState<WinOrLose>();
        }
        if (Data.TryFindEnemy())
        {
            Data.PlayAnimation(Data.Animations.Attack);
            Data.AttackEnemy();
            return;
        }
        Machine.ChangeState<Move>();
    }
}
