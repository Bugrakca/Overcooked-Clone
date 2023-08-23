using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
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
    private float _gamePlayingTimer = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("There is more than one GameManager instance!");
        }

        Instance = this;
        
        _state = State.WaitingToStart;
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
                }
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;

                if (_countdownToStartTimer < 0f)
                {
                    _state = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;

                if (_gamePlayingTimer < 0f)
                {
                    _state = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
        
        Debug.Log(_state);
    }

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }
}