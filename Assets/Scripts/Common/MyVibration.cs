namespace SquareDino
{
    public static class MyVibration
    {
        private static bool _isCanPlayVibro;
        public static void Haptic(MyHapticTypes hapticType)
        {
            if (Settings.GetOptionState(SettingTypes.Vibration)==false) return;
            VibrationHandler.Instance.AddVibration(hapticType);
        }
    }
}