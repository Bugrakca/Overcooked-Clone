using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("There is more than one MusicManager instance!");
        }

        Instance = this;
    }
}
