using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string Cut = "Cut";

    [SerializeField] private CuttingCounter cuttingCounter;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounterOnOnCut;
    }

    private void CuttingCounterOnOnCut (object sender, EventArgs e)
    {
        _animator.SetTrigger(Cut);
    }
}
