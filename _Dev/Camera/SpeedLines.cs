using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    private ParticleSystem _speedLinesEffect;
    private void Awake()
    {
        _speedLinesEffect = GetComponent<ParticleSystem>();
        EventManager.AddListener<PlayerRageEvent>(OnPlayerRage);
        EventManager.AddListener<PlayerFinishCrossedEvent>(OnFinishCrossed);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerRageEvent>(OnPlayerRage);
        EventManager.RemoveListener<PlayerFinishCrossedEvent>(OnFinishCrossed);
    }

    private void OnFinishCrossed(PlayerFinishCrossedEvent obj)
    {
        _speedLinesEffect.Stop();
    }

    private void OnPlayerRage(PlayerRageEvent obj)
    {
        _speedLinesEffect.Play();
    }
}
