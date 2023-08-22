using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public static event EventHandler OnPlayerMoveSound = delegate { };
    
    private Player _player;
    private float _footstepTimer;
    private float _footstepTimerMax = .1f;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _footstepTimer -= Time.deltaTime;
        if (_footstepTimer < 0f)
        {
            _footstepTimer = _footstepTimerMax;

            if (_player.IsWalking())
            {
                OnPlayerMoveSound.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
