using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
using SquareDino;
using UnityEngine.Events;

[RequireComponent(typeof(Button), typeof(Image))]
public class MyButton : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] protected UnityEvent _onPointerDown;
    public event System.Action OnClick;
    private bool interactable;
    private Image image;
    protected Button button;
    protected virtual void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        
        interactable = true;
        
        button.onClick.AddListener(ClickButton);
    }
    protected virtual void ClickButton()
    {
        if (!interactable) return;
        OnClick?.Invoke();
        MyVibration.Haptic(MyHapticTypes.LightImpact);
    }
    public void SetInteractable(bool value)
    {
        if (image == null) image = GetComponent<Image>();

        interactable = value;

        if (image != null)
        {
            image.color = interactable ? Color.white : Color.gray;
        }
    }
    public virtual void OnPointerDown(PointerEventData eventData) => _onPointerDown?.Invoke();
}