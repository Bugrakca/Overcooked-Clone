using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsRebindUI : MonoBehaviour
{
    [SerializeField] private Button listeningForInputField;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseUIText;
    [SerializeField] private Button interactButton;
    [SerializeField] private TextMeshProUGUI interactButtonUIText;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private TextMeshProUGUI interactAlternateButtonUIText;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private TextMeshProUGUI moveUpButtonUIText;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private TextMeshProUGUI moveDownButtonUIText;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private TextMeshProUGUI moveLeftButtonUIText;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private TextMeshProUGUI moveRightButtonUIText;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Button resetBindingsButton;

    private void Awake()
    {
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(InputReader.Binding.Interact, interactButton);
        });
        interactAlternateButton.onClick.AddListener(() =>
        {
            RebindBinding(InputReader.Binding.InteractAlternate, interactAlternateButton);
        });
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(InputReader.Binding.Pause, pauseButton);
        });
        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(InputReader.Binding.MoveUp, moveUpButton);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(InputReader.Binding.MoveDown, moveDownButton);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(InputReader.Binding.MoveLeft, moveLeftButton);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(InputReader.Binding.MoveRight, moveRightButton);
        });
        resetBindingsButton.onClick.AddListener(() =>
        {
            inputReader.ResetAllBindings();
            UpdateVisual();
            inputReader.SaveBindings();
        });
    }

    private void Start()
    {
        OptionsUI.Instance.OnControlsRebindButtonEvent += OptionsUI_OnControlsRebindButtonEvent;
        GameManager.Instance.OnGamePausedClose += (sender, args) => Hide();
        
        Hide();
        HideListeningForInputField();
    }

    private void OptionsUI_OnControlsRebindButtonEvent(object sender, EventArgs e)
    {
        Show();
        UpdateVisual();
    }

    private void ShowListeningForInputField()
    {
        listeningForInputField.gameObject.SetActive(true);
    }
    
    private void HideListeningForInputField()
    {
        listeningForInputField.gameObject.SetActive(false);
    }

    private void ShowRebindButton(Button button)
    {
        button.gameObject.SetActive(true);
    }

    private void HideRebindButton(Button button)
    {
        button.gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        pauseUIText.text = inputReader.GetBindingText(InputReader.Binding.Pause);
        interactButtonUIText.text = inputReader.GetBindingText(InputReader.Binding.Interact);
        interactAlternateButtonUIText.text = inputReader.GetBindingText(InputReader.Binding.InteractAlternate);
        moveUpButtonUIText.text = inputReader.GetBindingText(InputReader.Binding.MoveUp);
        moveDownButtonUIText.text = inputReader.GetBindingText(InputReader.Binding.MoveDown);
        moveLeftButtonUIText.text = inputReader.GetBindingText(InputReader.Binding.MoveLeft);
        moveRightButtonUIText.text = inputReader.GetBindingText(InputReader.Binding.MoveRight);
    }

    private void ChangeListeningInputFieldPosition(Button button)
    {
        listeningForInputField.transform.position = button.transform.position;
    }

    private void RebindBinding(InputReader.Binding binding, Button button)
    {
        ShowListeningForInputField();
        ChangeListeningInputFieldPosition(button);
        HideRebindButton(button);
        inputReader.RebindBinding(binding, () =>
        {
            ShowRebindButton(button);
            HideListeningForInputField();
            UpdateVisual();
        });

    }
}
