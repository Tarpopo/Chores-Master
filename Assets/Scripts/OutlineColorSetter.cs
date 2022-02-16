using System;
using UnityEngine;
public class OutlineColorSetter : MonoBehaviour
{
    private UnitSpawner _unitSpawner;
    private Line _line;
    private void Start()
    {
        _unitSpawner = GetComponentInParent<UnitSpawner>();
        FindObjectOfType<LevelManager>().OnLevelLoaded += OnStart;
    }
    public void OnStart()
    {
        _line = _unitSpawner.LinesCollection.GetClosestLine(transform.position);
        _line.SetActiveOutLineColor(true);
    }
    public void TrySetLineColor(Line line)
    {
        //var line=_unitSpawner.GetClosestLine(transform.position);
        if (_line == line) return;
        _line.SetActiveOutLineColor(false);
        _line = line;
        _line.SetActiveOutLineColor(true);
    }
}
