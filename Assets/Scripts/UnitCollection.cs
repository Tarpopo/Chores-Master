using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
public class UnitCollection : MonoBehaviour
{
    private Dictionary<int, List<UnitData>> _lineUnits = new Dictionary<int, List<UnitData>>();
    public List<UnitData> GetUnitList(int lineIndex)=>_lineUnits.ContainsKey(lineIndex)?_lineUnits[lineIndex]:null;
    public void CreateDictionary(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _lineUnits.Add(i,new List<UnitData>());
        }
    }
    public void ClearAllUnits() 
    {
        foreach (var value in _lineUnits.SelectMany(units => units.Value))
        {
            ManagerPool.Instance.Despawn(PoolType.Entities,value.gameObject);
        }
        _lineUnits.Clear();
    }
}
