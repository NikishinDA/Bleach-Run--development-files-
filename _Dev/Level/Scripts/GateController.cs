using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : InteractiveObject
{
    private OverGatesController _overGatesController;
    [SerializeField] private ParticleSystem _activationEffect;
    protected override void Awake()
    {
        base.Awake();
        
        transform.parent.parent.TryGetComponent(out _overGatesController);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (_overGatesController)
            _overGatesController.GatePassed();
        _activationEffect.Play();
    }
}
