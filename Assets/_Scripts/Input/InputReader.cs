using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions, PlayerInputActions.IMenuUIActions
{
    // Assign delegate{} to events to initialise them with an empty delegate
    // so we can skip the null check when we use them
    public event EventHandler<Vector2> MoveEvent = delegate { };
    public event EventHandler InteractEvent = delegate { };
    public event EventHandler InteractAlternate = delegate { };
    public event EventHandler PauseEvent = delegate { };
    public event EventHandler UnPauseEvent = delegate { };

    private PlayerInputActions _playerInput;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInputActions();
            _playerInput.Player.SetCallbacks(this);
            _playerInput.MenuUI.SetCallbacks(this);
#if UNITY_EDITOR
            SetGamePlay();
#endif
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(this, context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnInteractAlternative(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractAlternate.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            UnPauseEvent.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetGamePlay()
    {
        _playerInput.Player.Enable();
        _playerInput.MenuUI.Disable();
    }

    public void SetMenuUI()
    {
        _playerInput.MenuUI.Enable();
        _playerInput.Player.Disable();
    }

    public void DisableAllInput()
    {
        _playerInput.Player.Disable();
        _playerInput.MenuUI.Disable();
    }
}