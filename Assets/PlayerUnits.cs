using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "PlayerUnitsSettings")]
public class PlayerUnits : ScriptableObject
{
    [SerializeField] private List<PlayerUnitsSettings> _unitsSettings;
    private PlayerUnitsSettings GetSets(int levelIndex) =>
        _unitsSettings.FirstOrDefault(item => item.LevelIndex == levelIndex);
    public int GetRandomLevel(int levelIndex) => GetSets(levelIndex).GetRandomLevel();
}
[Serializable]
public class PlayerUnitsSettings
{
    public int LevelIndex;
    [SerializeField] private int _minUnitLevel;
    [SerializeField] private int _maxUnitLevel;
    public int GetRandomLevel() => Random.Range(_minUnitLevel, _maxUnitLevel+1);
}
