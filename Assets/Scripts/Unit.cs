using UnityEngine;
public class Unit : MonoBehaviour
{
    private UnitData _unitData;
    //public UnitData DataUnit => _unitData;
    private StateMachine<UnitData> _machine;
    public void InitializeMove()
    {
        _machine.Initialize<Move>();
    }
    private void Awake()
    {
        _unitData = GetComponent<UnitData>();
        _machine = new StateMachine<UnitData>();
        _machine.AddState(new Attack(_unitData,_machine));
        _machine.AddState(new Wait(_unitData,_machine));
        _machine.AddState(new Move(_unitData,_machine));
        _machine.AddState(new WinOrLose(_unitData,_machine));
    }
    private void Update()
    {
        _machine.CurrentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        _machine.CurrentState.PhysicsUpdate();
    }
}
