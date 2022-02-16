using System;
using System.Collections;
using UnityEngine;
public class NumbersCollector : MonoBehaviour
{
    [SerializeField] private int _frames;
    private UnitSpawner _unitSpawner;
    private void Start()
    {
        _unitSpawner = GetComponent<UnitSpawner>();
        var levelManager = FindObjectOfType<LevelManager>();
        levelManager.OnLevelLoaded += StopAllCoroutines;
        levelManager.OnLevelCompleted += StopAllCoroutines;
        levelManager.OnLevelNotPassed += StopAllCoroutines;
    }
    public void TryCollectNumbers(UnitData unit)
    {
        TryCollectVerticalNumbers(unit);
    }
    private void TryCollectVerticalNumbers(UnitData unit)
    {
        if (unit.IsMerging) return;
        var closeUnit = _unitSpawner.GetCloseUnit(unit,out var previous);
        if (closeUnit == null||unit.LevelUnit.Level != closeUnit.LevelUnit.Level) return;
        unit.IsMerging = true;
        var currentTransform =previous?closeUnit.transform:unit.transform;
        var targetPosition = previous?unit.transform.position:closeUnit.transform.position;
        StartCoroutine(MoveToUnit(currentTransform, targetPosition, _frames, () =>
        {
            closeUnit.LevelUnit.UpLevel();
            _unitSpawner.DespawnUnit(unit);
            closeUnit.ChangeScale();
        }));
    }
    private IEnumerator MoveToUnit(Transform currentTransform, Vector3 targetPosition, int frames,Action onEndMove)
    {
        var delta = (targetPosition - currentTransform.position)/frames;
        for (int i = 0; i < frames; i++)
        {
            currentTransform.position += delta;
            yield return Waiters.fixedUpdate;
        }
        onEndMove?.Invoke();
    }
    
}
