using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class LevelActions : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private List<LevelAction> _levelActions;
    private int _previousLevelIndex=-1;
    private void Start()
    {
        _levelManager.OnLevelLoaded += CheckAllEvents;
    }
    private void CheckAllEvents()
    {
        var levelIndex = Statistics.PlayerLevel;
        if (levelIndex == _previousLevelIndex) return;
        _previousLevelIndex = levelIndex;
        foreach (var levelAction in _levelActions) levelAction.TryLoadEvent(levelIndex);
    }
}
[Serializable]
public class LevelAction
{
    [SerializeField] private UnityEvent _onLevelLoad;
    [SerializeField] private int _levelIndex;
    public void TryLoadEvent(int levelIndex)
    {
        if(levelIndex==_levelIndex)_onLevelLoad?.Invoke();
    }
}
