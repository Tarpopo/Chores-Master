using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "UnitsSkins")]
public class UnitsSkins : ScriptableObject
{
    [SerializeField] private List<UnitSkinSettings> _unitsSkins;
    public UnitSkinSettings GetUnitSkin(int unitLevel)
    {
        return _unitsSkins.Select(skin => skin.TryGetSkin(unitLevel)).FirstOrDefault(newSkin => newSkin != null);
    }
}
[Serializable]
public class UnitSkinSettings
{
    public GameObject UnitSkin;
    public AllUnitAnimation Animations;
    //[SerializeField] private int _level;
    [SerializeField] private int[] _levels;
    public UnitSkinSettings TryGetSkin(int level)
    {
        return _levels.Any(item=>item==level) ? this : null;
    }
}
[Serializable]
public class AllUnitAnimation
{
    public UnitAnimations Attack;
    public UnitAnimations Idle;
    public UnitAnimations Run;
}
