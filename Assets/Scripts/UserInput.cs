using UnityEngine;
using UnityEngine.Events;
public class UserInput : MonoBehaviour
{
   // public bool IsMoved { get; private set;}
   // private float _minDistanceToMove;
   // private Vector2 _startPosition;
   [SerializeField] private UnityEvent _onTouchDown;
   public event UnityAction OnTouchDown
   {
      add => _onTouchDown.AddListener(value);
      remove => _onTouchDown.RemoveListener(value);
   }
   // [SerializeField] private UnityEvent _onSwipe;
   // public event UnityAction OnSwipe
   // {
   //    add => _onSwipe.AddListener(value);
   //    remove => _onSwipe.RemoveListener(value);
   // }
   [SerializeField] private UnityEvent _onTouchUp;
   public event UnityAction OnTouchUp
   {
      add => _onTouchUp.AddListener(value);
      remove => _onTouchUp.RemoveListener(value);
   }
   // [SerializeField] private UnityEvent _onTouch;
   // public event UnityAction OnTouch
   // {
   //    add => _onTouch.AddListener(value);
   //    remove => _onTouch.RemoveListener(value);
   // }
   private Camera _camera;
   public Vector2 TouchPosition => Input.mousePosition;
   public Vector2 TouchOnScreen => _camera.ScreenToViewportPoint(TouchPosition);
   public Ray ScreenRay => _camera.ScreenPointToRay(TouchPosition);
   private void Start()
   {
      _camera = Camera.main;
   }
   public Vector3 GetTouchOnWorld(float Zposition)
   {
      var touchPosition = TouchPosition;
      var position=new Vector3(touchPosition.x,touchPosition.y,Zposition);
      return _camera.ScreenToWorldPoint(position);
   }

   public Vector3 GetTouchOnPlane(Vector3 position)
   {
      var playerPlane = new Plane(Vector3.up, position);
      var ray = _camera.ScreenPointToRay (TouchPosition);
      return playerPlane.Raycast(ray, out var distToCam) ? ray.GetPoint(distToCam) : Vector3.zero;
   }

   private void Update()
   {
      if (GameState.Instance.IsActive==false) return;
      if(Input.GetMouseButtonDown(0))_onTouchDown?.Invoke();
      if(Input.GetMouseButtonUp(0))_onTouchUp?.Invoke();
   }
      // if (Input.touchCount <= 0||GamePause.Instance.IsPaused) return;
      // var touch = Input.GetTouch(0);
      // switch (touch.phase)
      // {
      //    case TouchPhase.Began:
      //    {
      //       _onTouchDown?.Invoke();
      //       _startPosition = touch.position;
      //       break;
      //    }
      //    case TouchPhase.Moved:
      //    {
      //       print("moved");
      //       IsMoved = true;
      //       break;
      //    }
      //    case TouchPhase.Ended:
      //    {
      //       _onTouchUp?.Invoke();
      //       IsMoved = false;
      //       break;
      //    }
      // }
}

