using System;
using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Rigidbody[] ragdoll;
    [SerializeField] private Collider[] ragdollColliders;
    private HighlightEffect _highlightEffect;
    //[SerializeField] private ParticleSystem rageEffect;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _highlightEffect = GetComponent<HighlightEffect>();
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<PlayerKilledMonsterEvent>(OnMonsterKilled);
        EventManager.AddListener<PlayerRageEvent>(OnRage);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<FinisherPlayerInPosition>(OnPlayerInPosition);
        EventManager.AddListener<FinisherPlayerSlashEvent>(OnPlayerSlash);
        EventManager.AddListener<PlayerApplyEffectEvent>(OnApplyEffect);
        EventManager.AddListener<BossHealthDepletedEvent>(OnBossHealthDepleted);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<PlayerKilledMonsterEvent>(OnMonsterKilled);
        EventManager.RemoveListener<PlayerRageEvent>(OnRage);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<FinisherPlayerInPosition>(OnPlayerInPosition);
        EventManager.RemoveListener<FinisherPlayerSlashEvent>(OnPlayerSlash);
        EventManager.RemoveListener<PlayerApplyEffectEvent>(OnApplyEffect);
        EventManager.RemoveListener<BossHealthDepletedEvent>(OnBossHealthDepleted);

    }

    private void OnBossHealthDepleted(BossHealthDepletedEvent obj)
    {
        _animator.SetTrigger("Fatality");
        _animator.ResetTrigger("Finisher Attack");
    }

    private void OnApplyEffect(PlayerApplyEffectEvent obj)
    {
        if (obj.Type == InteractiveType.Monster)
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void OnPlayerSlash(FinisherPlayerSlashEvent obj)
    {
        _animator.SetTrigger("Finisher Attack");
    }

    private void OnPlayerInPosition(FinisherPlayerInPosition obj)
    {
        _animator.SetTrigger("Finisher");
    }

    private void OnGameOver(GameOverEvent obj)
    {
        if (!obj.IsWin)
        {
            _animator.enabled = false;
            foreach (var ragdollCollider in ragdollColliders)
            {
                ragdollCollider.enabled = true;
            }
            foreach (var rb in ragdoll)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddExplosionForce(10f, transform.position + Vector3.forward, 10f, 0, ForceMode.Impulse);
            }

        }
    }

    private void OnRage(PlayerRageEvent obj)
    {
        _animator.SetTrigger("Rage");
        _highlightEffect.enabled = true;
        //rageEffect.Play();
    }

    private void OnMonsterKilled(PlayerKilledMonsterEvent obj)
    {
        _animator.SetTrigger("Attack");
    }

    private void OnGameStart(GameStartEvent obj)
    {
        _animator.SetTrigger("Start");
    }

    private void FinisherPlayerHit()
    {
        EventManager.Broadcast(GameEventsHandler.FinisherPlayerActualHitEvent);
    }

    private void FatalityHit()
    {
        EventManager.Broadcast(GameEventsHandler.FinisherEndEvent);
    }
}
