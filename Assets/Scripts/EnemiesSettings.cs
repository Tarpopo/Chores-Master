using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(menuName = "EnemiesSettings")]
public class EnemiesSettings : SerializedScriptableObject
{
    [SerializeField] private Dictionary<int, List<UnitStats>> _enemiesSettings;
    public List<UnitStats> GetEnemiesOnLevel(int levelIndex)
    {
        return _enemiesSettings.ContainsKey(levelIndex)?_enemiesSettings[levelIndex]:null;
    }
}
