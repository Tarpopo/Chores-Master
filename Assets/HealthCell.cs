using UnityEngine;
using UnityEngine.UI;
public class HealthCell : MonoBehaviour
{
    [SerializeField] private Sprite _activeCell;
    [SerializeField] private Sprite _deactiveCell;
    private Image _image;
    private void Awake()=>_image = GetComponent<Image>();
    public void SetActiveCell(bool isActive)=>_image.sprite = isActive ? _activeCell : _deactiveCell;
}
