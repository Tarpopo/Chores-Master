using System;
using SquareDino;
using UnityEngine;
using UnityEngine.Events;
public class UnitLevel : MonoBehaviour
{
    [SerializeField] private UnityEvent<UnitData> _onLevelEnd;
    public event UnityAction<UnitData> OnLevelEnd
    {
        add => _onLevelEnd.AddListener(value);
        remove => _onLevelEnd.RemoveListener(value);
    }
    [SerializeField] private UnityEvent<UnitData> _onLevelReduce;
    public event UnityAction<UnitData> OnLevelReduce
    {
        add => _onLevelReduce.AddListener(value);
        remove => _onLevelReduce.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _onLevelUp;
    public event UnityAction OnLevelUp
    {
        add => _onLevelUp.AddListener(value);
        remove => _onLevelUp.RemoveListener(value);
    }
    public int Level { get; private set;}
    public bool IsHaveLevel => Level > 0;
    [SerializeField]private WorldText _text;
    public void UpLevel()
    {
        Level++;
        MyVibration.Haptic(MyHapticTypes.LightImpact);
        UpdateText();
        _onLevelUp?.Invoke();
    }
    public void SetUnitLevel(int level)
    {
        if (level > 11) return;
        Level = level;
        UpdateText();
    }
    public void ReduceLevel()
    {
        if (IsHaveLevel == false) return;
        Level--;
        UpdateText();
        if (IsHaveLevel == false)
        {
            MyVibration.Haptic(MyHapticTypes.Selection);
            _onLevelEnd?.Invoke(GetComponent<UnitData>());
            return;
        }
        _onLevelReduce?.Invoke(GetComponent<UnitData>());
    }

    private void OnDisable()
    {
        _onLevelEnd.RemoveAllListeners();
        _onLevelReduce.RemoveAllListeners();
    }

    private void UpdateText()
    {
        _text.UpdateText(Math.Pow(2, Level).ToString());
    }
    public void OnStart()
    {
        UpdateText();
    }
}
