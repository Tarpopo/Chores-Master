using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private RectTransform _head;
    [SerializeField] private bool _isAnimationScale;
    [SerializeField] private float _scale;
    [SerializeField] private int _scaleSpeed;
    private float _width;
    private float _halfWidth;
    private float _maxAmount=1;
    private void Start()
    {
        _width = _progressBar.rectTransform.sizeDelta.x;
        _halfWidth = _width / 2;
    }
    private void UpdateHeadPosition()
    {
        _head.localPosition = new Vector2(_width * _progressBar.fillAmount - _halfWidth, _head.localPosition.y);
    }
    private float CalculateValue(float count) => Mathf.InverseLerp(0, _maxAmount,count);
    public void SetParameters(float maxAmount,float startValue)
    {
        _maxAmount = maxAmount;
        SetProgress(startValue);
    }
    public void SetProgress(float amount)
    {
        _progressBar.fillAmount = CalculateValue(amount);
        if (_isAnimationScale && _progressBar.fillAmount >= 1)
            StartCoroutine(CorroutinesKid.ScaleAnimation(transform, _scale*Vector3.one,
                _scaleSpeed, _scaleSpeed, null));
        if(_head!=null)UpdateHeadPosition();
    }
}
