using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            if (!GameManager.Instance.IsGamePaused())
            {
                GameManager.Instance.PauseGame();
            }
            else
            {
                GameManager.Instance.UnPauseGame();
            }
        });
        mainMenuButton.onClick.AddListener((() => Loader.Load(Loader.Scene.MainMenuScene)));
        optionsButton.onClick.AddListener(() => {OptionsUI.Instance.Show();});
        
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManagerOnGamePaused;
        GameManager.Instance.OnGamePausedClose += GamaManagerOnGameUnPaused;
    
        Hide();
    }

    private void GameManagerOnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void GamaManagerOnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }


    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}