using SquareDino;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class OptionButton : MyButton
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite onImage;
    [SerializeField] private Sprite offImage;
    [SerializeField] private SettingTypes _option;
    [SerializeField] private UnityEvent<bool> _onValueChanged;
    protected override void ClickButton()
    {
        base.ClickButton();
        Settings.SetOption(_option,!Settings.GetOptionState(_option));
        if (Settings.GetOptionState(_option)) MyVibration.Haptic(MyHapticTypes.LightImpact);
        _onValueChanged?.Invoke(Settings.GetOptionState(_option));
        ChangeImage();
    }
    private void ChangeImage()=>targetImage.sprite = Settings.GetOptionState(_option) ? onImage : offImage;
    private void Start()=>ChangeImage();
}