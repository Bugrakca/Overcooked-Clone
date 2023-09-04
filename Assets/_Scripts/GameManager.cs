using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public event EventHandler OnStateChanged = delegate { };
    public event EventHandler OnGamePaused = delegate { };
    public event EventHandler OnGamePausedClose = delegate { };
    
    public static GameManager Instance { get; private set; }

    [SerializeField] private InputReader inputReader;

    private bool _isGamePaused;
    
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State _state;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 10f;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("There is more than one GameManager instance!");
        }

        Instance = this;
        
        _state = State.WaitingToStart;
    }

    private void Start()
    {
        inputReader.PauseEvent += InputReaderOnPauseEvent;
        inputReader.UnPauseEvent += InputReaderOnUnPauseEvent;
    }

    //UnSubscribe all the events when its destroyed or change the scenes. Purpose of "Missing Reference Exception".
    private void OnDisable()
    {
        inputReader.PauseEvent -= InputReaderOnPauseEvent;
        inputReader.UnPauseEvent -= InputReaderOnUnPauseEvent;
    }

    private void InputReaderOnPauseEvent(object sender, EventArgs e)
    {
        PauseGame();
    }

    private void InputReaderOnUnPauseEvent(object sender, EventArgs e)
    {
        UnPauseGame();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;

                if (_waitingToStartTimer < 0f)
                {
                    _state = State.CountdownToStart;
                    OnStateChanged.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;

                if (_countdownToStartTimer < 0f)
                {
                    _state = State.GamePlaying;
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    OnStateChanged.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;

                if (_gamePlayingTimer < 0f)
                {
                    _state = State.GameOver;
                    OnStateChanged.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
        
        Debug.Log(_state);
    }

    public void PauseGame()
    {
        // inputReader.PauseEvent -= InputReaderOnPauseEvent;
        Time.timeScale = 0f;
        OnGamePaused.Invoke(this, EventArgs.Empty);
        _isGamePaused = true;

        inputReader.SetMenuUI();
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        // inputReader.PauseEvent += InputReaderOnPauseEvent;
        OnGamePausedClose.Invoke(this, EventArgs.Empty);
        _isGamePaused = false;
        
        inputReader.SetGamePlay();
    }

    public bool IsGamePaused()
    {
        return _isGamePaused;
    }

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return _state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return _state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (_gamePlayingTimer / _gamePlayingTimerMax);
    }
}