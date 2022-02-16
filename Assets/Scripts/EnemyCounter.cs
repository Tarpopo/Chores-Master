using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class EnemyCounter : MonoBehaviour
{
    [SerializeField] private UnityEvent _onEnemiesLeft;
    public event UnityAction OnEnemiesLeft
    {
        add => _onEnemiesLeft.AddListener(value);
        remove => _onEnemiesLeft.RemoveListener(value);
    }
    [SerializeField] private TMP_Text _count;
    public int EnemyCount => _enemyCount;
    public int EndEnemyCount => _endEnemyCount;
    private int _enemyCount;
    private int _endEnemyCount;
    private string _countTemplate;
    private bool _isHaveEnemy => _enemyCount < _endEnemyCount;
    private void UpdateCountText()=>_count.text = _enemyCount+_countTemplate;
    public void SetEnemyCount(int count)
    {
        _enemyCount = 0;
        _endEnemyCount=count;
        _countTemplate = "/"+count;
        UpdateCountText();
    }
    public void ReduceEnemyCount()
    { 
        if (_isHaveEnemy == false) return; 
        _enemyCount++;
        UpdateCountText();
        if (_isHaveEnemy == false) _onEnemiesLeft?.Invoke();
    }
}
