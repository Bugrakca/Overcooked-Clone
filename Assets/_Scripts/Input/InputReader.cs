using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    public event EventHandler<Vector2> MoveEvent;

    public event EventHandler InteractEvent;
    
    private PlayerInputActions _playerInput;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInputActions();
            _playerInput.Player.SetCallbacks(this);
            SetGamePlay();
        }
    }

    public void OnMove (InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(this, context.ReadValue<Vector2>());
    }

    public void OnInteract (InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetGamePlay()
    {
        _playerInput.Player.Enable();
        //Disable other input maps.
    }
}