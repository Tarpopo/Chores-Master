using DefaultNamespace;
using UnityEngine;
public class InteractiveGetter : MonoBehaviour
{
    private UserInput _userInput;
    private IInteractive _currentInteractive;
    private void Start()
    {
        _userInput = GetComponent<UserInput>();
        _userInput.OnTouchDown += StartMove;
        _userInput.OnTouchUp += () => _currentInteractive = null;
    }
    private void StartMove()
    {
        var ray = _userInput.ScreenRay;
        _currentInteractive = null;
        if (Physics.Raycast(ray, out var hit))
        {
            _currentInteractive= hit.collider.GetComponentInParent<IInteractive>();
            _currentInteractive?.StartInteractive(_userInput);
        }
    }
    private void FixedUpdate()
    {
        _currentInteractive?.UpdateInteractive(_userInput);
    }
}
