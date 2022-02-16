using UnityEngine;
public static class Settings
{
    public static void SetOption(SettingTypes setting,bool isActive)=>PlayerPrefs.SetInt(setting.ToString(), isActive ? 1 : 0);
    public static bool GetOptionState(SettingTypes setting)=>PlayerPrefs.GetInt(setting.ToString(), 1) == 1;
}
public enum SettingTypes
{
    Vibration,
    Sounds
}