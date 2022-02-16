using UnityEngine;
using UnityEngine.Events;
public class UnitHealth : MonoBehaviour
{
    [SerializeField] private ProgressBar _healthBar;
    private int _health;
    private bool _isDead => _health <= 0;
    [SerializeField] private UnityEvent _onHealthEnd;
    public event UnityAction OnHealthEnd
    {
        add => _onHealthEnd.AddListener(value);
        remove => _onHealthEnd.RemoveListener(value);
    }
    [SerializeField] private UnityEvent<UnitData> _onTakeDamage;
    public event UnityAction<UnitData> OnTakeDamage
    {
        add => _onTakeDamage.AddListener(value);
        remove => _onTakeDamage.RemoveListener(value);
    }
    public void SetHealth(int health)
    {
        if (health > 11) return;
        _health = health;
        _healthBar.SetParameters(health,health);
    }
    public void TakeDamage()
    {
        if (_isDead) return;
        _health--;
        _healthBar.SetProgress(_health);
        if (_isDead)
        {
            _onHealthEnd?.Invoke();
            return;
        }
        _onTakeDamage?.Invoke(GetComponent<UnitData>());
    }
}
