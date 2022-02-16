using System;
using System.Collections.Generic;
using SquareDino.Scripts.MyAnalytics;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnDelay;
    [SerializeField] private EnemiesSettings _enemiesSettings;
    [SerializeField] private EnemyCounter _enemyCounter;
    [SerializeField] private float _startDelay;
    [SerializeField] private float _winDelay;
    [SerializeField] private float _loseDelay;
    private List<UnitStats> _enemyUnits=new List<UnitStats>();
    private Timer _spawnTimer;
    private UnitSpawner _unitSpawner;
    private LevelManager _levelManager;
    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _levelManager.OnLevelLoaded += () =>
        {
            _enemyUnits = new List<UnitStats>(_enemiesSettings.GetEnemiesOnLevel(Statistics.CurrentLevelNumber + 1));
            _enemyCounter.SetEnemyCount(_enemyUnits.Count);
            Invoke(nameof(TrySpawnEnemy),_startDelay);
        };
        _enemyCounter.OnEnemiesLeft += ()=>
        {
            GameState.Instance.SetGameState(GameStates.Win);
            Invoke(nameof(CompleteGame),_winDelay);
        };
        _levelManager.OnLevelNotPassed += () =>_enemyUnits.Clear();
        _unitSpawner = GetComponent<UnitSpawner>();
        _unitSpawner.OnUnitDespawn += _enemyCounter.ReduceEnemyCount;
        _spawnTimer = new Timer();
    }
    private void TrySpawnEnemy()
    {
        if (_enemyUnits.Count == 0) return;
        var unit = _unitSpawner.SpawnUnit(_enemyUnits[0]);
        unit.OnCameToEnd += () =>
        {
            if (GameState.Instance.IsLose) return;
            GameState.Instance.SetGameState(GameStates.Lose);
            _unitSpawner.LinesCollection.GetLine(unit.LineIndex).SetOutLineColor(Color.red);
            Invoke(nameof(LoseGame),_loseDelay);
        };
        //unit.OnAlmostCameToEnd+=()=>_unitSpawner.LinesCollection.GetLine(unit.LineIndex).SetOutLineColor(Color.red);
        _enemyUnits.Remove(_enemyUnits[0]);
        _spawnTimer.StartTimer(_spawnDelay,TrySpawnEnemy);
    }
    private void CompleteGame()
    {
        _levelManager.CurrentLevel_OnLevelCompleted();
    }
    private void LoseGame()
    {
        MyAnalyticsManager.LevelFailed(100/_enemyCounter.EndEnemyCount*_enemyCounter.EnemyCount);
        _levelManager.CurrentLevel_OnLevelLosing();
    }
    private void Update()=>_spawnTimer.UpdateTimer();
}
[Serializable]
public class UnitStats
{
    public int LineIndex;
    public int UnitLevel;
}
