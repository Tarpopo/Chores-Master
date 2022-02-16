using UnityEngine;
public class Move : State<UnitData>
{
    private float _distanceToUnit;
    public Move(UnitData data, StateMachine<UnitData> stateMachine) : base(data, stateMachine) { }
    public override void Enter()
    {
        Data.PlayAnimation(Data.Animations.Run);
        _distanceToUnit = Data.IsMain ? 1.9f : Data.DistanceToUnit;
    }
    public override void PhysicsUpdate()
    {
        if (GameState.Instance.IsWinOrLose)
        {
            Data.PlayWinOrLoseAnimation(GameState.Instance.IsWin);
            Machine.ChangeState<WinOrLose>();
        }
        if(Data.TryFindEnemy())Machine.ChangeState<Attack>();
        var distance = Vector3.Distance(Data.UnitPosition, Data.TargetPosition);
        if (distance > _distanceToUnit)
        {
            //if(Data.IsMain&&distance<Data.DistanceToAlmost) Data.OnCameAlmost();
            Data.MoveToTarget();
            return;
        }
        if (Data.IsMain)
        {
            Data.CameToEnd();
            Machine.ChangeState<WinOrLose>();
        }
        else Machine.ChangeState<Wait>();
    }
}
