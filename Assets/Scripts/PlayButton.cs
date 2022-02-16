using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour,IPointerDownHandler
{
   [SerializeField] private UnityEvent _onStartPlay;
   public event UnityAction OnStartPlay
   {
      add => _onStartPlay.AddListener(value);
      remove => _onStartPlay.RemoveListener(value);
   }
   private void Start()=> SetActive();
   public void SetActive()
   {
      gameObject.SetActive(true);
      GameState.Instance.SetGameState(GameStates.Pause);
   }
   public void OnPointerDown(PointerEventData eventData)
   {
      GameState.Instance.SetGameState(GameStates.Active);
      _onStartPlay?.Invoke();
      gameObject.SetActive(false);
   }
}
