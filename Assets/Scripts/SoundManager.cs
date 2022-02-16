using DefaultNamespace;
using UnityEditor;
using UnityEngine;
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SoundsSettings _soundsSettings;
    public void PlaySound(SoundTypes soundType)
    {
        if(Settings.GetOptionState(SettingTypes.Sounds)==false)return;
        var sound = _soundsSettings.GetSound(soundType);
        _audioSource.PlayOneShot(sound.GetClip(),sound.Volume);
    }
}
