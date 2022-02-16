using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UIStartTimer : MonoBehaviour
{
    [SerializeField] private List<UIStartItem> _startItems;
    [SerializeField] private bool _playOnStart;
    [SerializeField] private bool _playOnLoad;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _upSpeed;
    [SerializeField] private int _downSpeed;
    private UIStartItem _currentItem;
    private void Start()
    {
        gameObject.SetActive(false);
        if (_playOnStart) StartTimer();
    }
    public void StartTimer()
    {
        if (_playOnLoad)
        {
            FindObjectOfType<LevelManager>().OnLevelLoaded += StartTimer;
            _playOnLoad = false;
        }
        gameObject.SetActive(true);
        GameState.Instance.SetGameState(GameStates.Pause);
        _currentItem = _startItems[0];
        _text.text = _currentItem.Text;
        StartCoroutine(CorroutinesKid.ScaleAnimation(_text.transform,Vector3.one*_currentItem.Scale,_upSpeed,_downSpeed, GetNextItem));
    }
    private void GetNextItem()
    {
        if (_startItems.TryGetNextItem(_currentItem, out _currentItem) == false)
        {
            GameState.Instance.SetGameState(GameStates.Active);
            _text.text = string.Empty;
            gameObject.SetActive(false);
            return;
        }
        _text.text = _currentItem.Text;
        StartCoroutine(CorroutinesKid.ScaleAnimation(_text.transform,Vector3.one*_currentItem.Scale,_upSpeed,_downSpeed, GetNextItem));
    }
}
[Serializable]
public class UIStartItem
{
    public string Text;
    public float Scale;
}

