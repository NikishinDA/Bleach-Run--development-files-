using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator _animator;
    private bool _isUpperEffect;
    [SerializeField] private ParticleSystem lowerEffect;
    [SerializeField] private ParticleSystem upperEffect;
    private CinemachineImpulseSource _impulseSource;
    [SerializeField] private ParticleSystem bossDeathEffect;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        EventManager.AddListener<FinisherPlayerActualHitEvent>(OnPlayerActualHit);
        EventManager.AddListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.AddListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<FinisherPlayerActualHitEvent>(OnPlayerActualHit);
        EventManager.RemoveListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.RemoveListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);

    }

    private void OnGameOver(GameOverEvent obj)
    {
        //TBD
    }

    private void OnPlayerOnPosition(FinisherPlayerInPosition obj)
    {
        _animator.SetTrigger("Attack");
    }

    private void OnFinisherEnd(FinisherEndEvent obj)
    {
        bossDeathEffect.Play();
        gameObject.SetActive(false);
    }

    private void OnPlayerActualHit(FinisherPlayerActualHitEvent obj)
    {
        //_animator.SetTrigger("Hit");
        if (_isUpperEffect)
        {
            upperEffect.Play();
        }
        else
        {
            lowerEffect.Play();
        }

        _isUpperEffect = !_isUpperEffect;
        _impulseSource.GenerateImpulse();
    }
    
    private void BossAttackAnimationEvent()
    {
        EventManager.Broadcast(GameEventsHandler.FinisherBossAttackEvent);
    }
}
