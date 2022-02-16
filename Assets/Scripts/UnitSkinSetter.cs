using System;
using DefaultNamespace;
using UnityEngine;
public class UnitSkinSetter : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private UnitsSkins _unitsSkins;
    [NonSerialized] public GameObject CurrentSkin;
    [NonSerialized] public UnitSkinSettings SkinSettings;
    public void ChangeSkin(int unitLevel)
    {
        if (CurrentSkin!=null)
        {
            CurrentSkin.transform.SetParent(null);
            ManagerPool.Instance.Despawn(PoolType.Entities,CurrentSkin);
        }
        SkinSettings=_unitsSkins.GetUnitSkin(unitLevel);
        CurrentSkin = SkinSettings.UnitSkin;
        CurrentSkin = ManagerPool.Instance.Spawn(PoolType.Entities,CurrentSkin);
        CurrentSkin.transform.SetParent(transform);
        CurrentSkin.transform.localPosition=Vector3.zero;
        CurrentSkin.transform.localRotation = Quaternion.Euler(0,0,0);
        CurrentSkin.transform.localScale = Vector3.one;
    }
}
