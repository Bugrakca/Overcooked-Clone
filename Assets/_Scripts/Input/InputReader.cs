using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions, PlayerInputActions.IMenuUIActions
{
    private const string PlayerPrefsBindings = "InputBindings";
    
    // Assign delegate{} to events to initialise them with an empty delegate
    // so we can skip the null check when we use them
    public event EventHandler<Vector2> MoveEvent = delegate { };
    public event EventHandler InteractEvent = delegate { };
    public event EventHandler InteractAlternate = delegate { };
    public event EventHandler PauseEvent = delegate { };
    public event EventHandler UnPauseEvent = delegate { };

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause
    }

    private PlayerInputActions _playerInput;

    // private void Awake()
    // {
    //     if (PlayerPrefs.HasKey(PlayerPrefsBindings))
    //     {
    //         _playerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PlayerPrefsBindings));
    //     }
    // }

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInputActions();
            if (PlayerPrefs.HasKey(PlayerPrefsBindings))
            {
                _playerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PlayerPrefsBindings));
            }
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

    public string GetBindingText(Binding binding)
    {
        var moveInput = _playerInput.Player.Move.bindings;

        switch (binding)
        {
            default:
            case Binding.Interact:
                return _playerInput.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return _playerInput.Player.InteractAlternative.bindings[0].ToDisplayString();
            case Binding.Pause:
                return _playerInput.Player.Pause.bindings[0].ToDisplayString();
            case Binding.MoveUp:
                return moveInput[1].ToDisplayString();
            case Binding.MoveDown:
                return moveInput[2].ToDisplayString();
            case Binding.MoveLeft:
                return moveInput[3].ToDisplayString();
            case Binding.MoveRight:
                return moveInput[4].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        DisableAllInput();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Interact:
                inputAction = _playerInput.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = _playerInput.Player.InteractAlternative;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _playerInput.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.MoveUp:
                inputAction = _playerInput.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = _playerInput.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = _playerInput.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = _playerInput.Player.Move;
                bindingIndex = 4;
                break;
        }

        if (binding == Binding.Pause)
        {
            inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
            {
                callback.Dispose();
                _playerInput.Player.Enable();
                _playerInput.MenuUI.Enable();
                onActionRebound();
                _playerInput.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString(PlayerPrefsBindings, _playerInput.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

            }).WithAction(_playerInput.MenuUI.UnPause).Start();
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            _playerInput.Player.Enable();
            onActionRebound();
            PlayerPrefs.SetString(PlayerPrefsBindings, _playerInput.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }).Start();
    }
}