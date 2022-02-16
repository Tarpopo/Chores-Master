using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SettingButton : MyButton
{
    [SerializeField] private CanvasGroup childCanvas;
    [SerializeField] private HideSettingsButton _settingsButton;
    [SerializeField] private UnityEvent _onSettingsClose;
    private bool isActivated;
    private Coroutine _coroutine;

    // protected override void Awake()
    // {
    //     base.Awake();
    //     _settingsButton.OnClick += CloseSettings;
    // }

    public void CloseSettings()
    {
        if (_coroutine != null) return;
        isActivated = false;
        _coroutine=StartCoroutine(ChangeAlpha(0, 15, () =>
        {
            _onSettingsClose?.Invoke();
            SetEnableClick(false);
        }));
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (_coroutine != null) return;
        _onPointerDown?.Invoke();
    }
    protected override void ClickButton()
    {
        if (_coroutine != null) return;
        base.ClickButton();
        isActivated = !isActivated;
        if (isActivated)
        {
            _coroutine=StartCoroutine(ChangeAlpha(1, 15, () =>
            {
                SetEnableClick(true);
            }));
            //childCanvas.DOFade(1f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() => SetEnableClick(true));
        }
        else
        {
            //childCanvas.DOFade(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() => SetEnableClick(false));
            _coroutine=StartCoroutine(ChangeAlpha(0, 15, () =>
            {
                SetEnableClick(false);
            }));
        }   
    }

    private IEnumerator ChangeAlpha(float target, int frames, Action onEnd)
    {
        var step=(target-childCanvas.alpha)/frames;
        for (int i = 0; i < frames; i++)
        {
            childCanvas.alpha += step;
            yield return null;
        }
        _coroutine = null;
        onEnd?.Invoke();
    }

    private void SetEnableClick(bool value)
    {
        childCanvas.interactable = value;
        childCanvas.blocksRaycasts = value;
    }
}