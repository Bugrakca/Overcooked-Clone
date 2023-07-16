using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OpenClose = "OpenClose";
    //Cached property index
    private static readonly int OpenClosed = Animator.StringToHash(OpenClose);
    

    [SerializeField] private ContainerCounter containerCounter;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounterOnOnPlayerGrabbedObject;
    }

    private void ContainerCounterOnOnPlayerGrabbedObject (object sender, EventArgs e)
    {
        _animator.SetTrigger(OpenClosed);
    }
}
