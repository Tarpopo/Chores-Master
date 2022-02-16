using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="SoundSettings")]
public class SoundsSettings : ScriptableObject
{
    [SerializeField]private List<Sound> _sounds;
    public Sound GetSound(SoundTypes soundType)=>_sounds.FirstOrDefault(item=>item.SoundType==soundType);
}
[Serializable]
public class Sound
{
    public SoundTypes SoundType;
    public AudioClip AudioClip;
    [SerializeField] private List<AudioClip> _clips;
    public float Volume;
    public AudioClip GetClip()
    {
        return _clips.Count > 0 ? _clips[Random.Range(0, _clips.Count)] : AudioClip;
    }
}
public enum SoundTypes
{
    SwordAttack,
    Sheath,
    Drums,
    CannonShoot,
    Win,
    Lose
}