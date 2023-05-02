using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem upgradeEffect;
    [SerializeField] private ParticleSystem fireEffect;
    [SerializeField] private ParticleSystem hitEffect;

    private void Awake()
    {
        EventManager.AddListener<PlayerStageChangeEvent>(OnStageChange);
        EventManager.AddListener<PlayerApplyEffectEvent>(OnApplyEffect);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerStageChangeEvent>(OnStageChange);
        EventManager.RemoveListener<PlayerApplyEffectEvent>(OnApplyEffect);

        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
    }

    private void OnGameOver(GameOverEvent obj)
    {
        if (!obj.IsWin)
        {
            hitEffect.Play();
        }
    }


    private void OnApplyEffect(PlayerApplyEffectEvent obj)
    {
        if (obj.Type == InteractiveType.Fire)
        {
            fireEffect.Play();
        }
        else if (obj.Type == InteractiveType.Monster)
        {
            hitEffect.Play();
        }
    }

    private void OnStageChange(PlayerStageChangeEvent obj)
    {
        upgradeEffect.Play();
    }
}
