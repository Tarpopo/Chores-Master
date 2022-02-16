using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private UnityEvent _onUnitDespawn; 
    public event UnityAction OnUnitDespawn
    {
        add => _onUnitDespawn.AddListener(value);
        remove => _onUnitDespawn.RemoveListener(value);
    }
    [SerializeField] private GameObject _unit;
    [SerializeField] private bool _isEnemySpawner;
    private NumbersCollector _numbersCollector;
    public Lines LinesCollection { get; private set;}
    private UnitCollection _unitCollection;
    private void Awake()
    {
        _numbersCollector = GetComponent<NumbersCollector>();
        _unitCollection = GetComponent<UnitCollection>();
        ManagerPool.Instance.AddPool(PoolType.Entities).PopulateWith(_unit, 15);
        var levelManager = FindObjectOfType<LevelManager>();
        //levelManager.OnLevelCompleted += _unitCollection.ClearAllUnits;
        //levelManager.OnLevelNotPassed += _unitCollection.ClearAllUnits;
        levelManager.OnLevelLoaded +=()=>
        {
            _unitCollection.ClearAllUnits();
            LinesCollection = FindObjectOfType<Lines>();
            _unitCollection.CreateDictionary(LinesCollection.LineCount);
        };
    }
    public UnitData SpawnUnit(UnitStats unitStats)
    {
        if (LinesCollection.LineCount == 0) return null;
        var unitsList = _unitCollection.GetUnitList(unitStats.LineIndex);
        if (unitsList == null) return null;
        var isMain = unitsList.Count == 0;
        var unit = ManagerPool.Instance.Spawn<UnitData>(PoolType.Entities,_unit,LinesCollection.GetLine(unitStats.LineIndex).GetSpawnPoint(_isEnemySpawner));
        var target = unitsList.Count == 0
            ? LinesCollection.GetLine(unitStats.LineIndex).GetTargetTransform(_isEnemySpawner)
            : unitsList.GetLastElement().transform;
        unit.transform.LookAt(LinesCollection.GetLine(unitStats.LineIndex).GetTargetTransform(_isEnemySpawner));
        unit.OnUnitStand += _numbersCollector.TryCollectNumbers;
        unit.LevelUnit.OnLevelReduce += _numbersCollector.TryCollectNumbers;
        unit.LevelUnit.OnLevelEnd += DespawnUnit;
        unit.SetParameters(unitStats.LineIndex,unitStats.UnitLevel,isMain,target);
        unitsList.Add(unit);
        return unit;
    }
    public UnitData GetCloseUnit(UnitData unit,out bool previous)
    {
        previous = false;
        if (_unitCollection.GetUnitList(unit.LineIndex).TryGetPreviousItem(unit, out var element) == false)
        {
            if (_unitCollection.GetUnitList(unit.LineIndex).TryGetNextItem(unit, out element)) previous = true;
        }
        return element==unit?null:element;
    }
    public void DespawnUnit(UnitData unit)
    {
        var currentLine = _unitCollection.GetUnitList(unit.LineIndex);
        // print(unit.GetHashCode());
        // print("tryDeleteElementOnLine "+unit.LineIndex+":"+currentLine.Count+currentLine.Contains(unit));
        if (currentLine.Count!=0&&currentLine.TryGetNextItem(unit,out var element))
        {
            element.SetTargetTransform(currentLine.TryGetPreviousItem(unit, out var previous)
                ? previous.TargetTransform
                : LinesCollection.GetLine(unit.LineIndex).GetTargetTransform(_isEnemySpawner));
            if(unit.IsMain)element.SetUnitMain();
        }
        // unit.OnUnitStand -= _numbersCollector.TryCollectNumbers;
        // unit.LevelUnit.OnLevelReduce -= _numbersCollector.TryCollectNumbers;
        // unit.LevelUnit.OnLevelEnd -= DespawnUnit;
        currentLine.Remove(unit);
        ManagerPool.Instance.Despawn(PoolType.Entities,unit.gameObject);
        _onUnitDespawn?.Invoke();
    }
    // public List<UnitData> GetAllHorizontalUnits(UnitData unit,float minDistance,float zPosition)
    // {
    //     var list = new List<UnitData>();
    //     foreach (var lineIndex in LinesCollection.GetThreeCenterIndex(unit.LineIndex))
    //     {
    //         var unitData = _unitCollection.GetUnitList(lineIndex).GetClosestElementOnZ(minDistance, zPosition);
    //         if(unitData!=null&&unit.LevelUnit.Level==unitData.LevelUnit.Level) list.Add(unitData);
    //     }
    //     return list;
    // }
}
