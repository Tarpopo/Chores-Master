using UnityEngine;
public class Wait : State<UnitData>
{ 
    public Wait(UnitData data, StateMachine<UnitData> stateMachine) : base(data, stateMachine){}
    public override void Enter()
    {
        Data.OnUnitWait();
        Data.PlayAnimation(Data.Animations.Idle);
    }
    public override void PhysicsUpdate()
    {
        if(Data.IsMain&&Data.TryFindEnemy())Machine.ChangeState<Attack>();
        if (GameState.Instance.IsWinOrLose)
        {
            Data.PlayWinOrLoseAnimation(GameState.Instance.IsWin);
            Machine.ChangeState<WinOrLose>();
        }
        if (Vector3.Distance(Data.UnitPosition, Data.TargetPosition) > Data.DistanceToUnit) Machine.ChangeState<Move>();
    }
}
