using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinisherPlayerHealthManager : MonoBehaviour
{
    [SerializeField] private float fullHealth = 100f;

    [SerializeField]
    private float damagePerHit = 20f;

    private float _currentHealth;

    private HealthBarCanvasController _healthBarCanvasController;
    private void Awake()
    {
        EventManager.AddListener<FinisherBossAttackEvent>(OnBossAttack);
        _currentHealth = fullHealth;
        _healthBarCanvasController = GetComponent<HealthBarCanvasController>();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<FinisherBossAttackEvent>(OnBossAttack);
    }

    private void OnBossAttack(FinisherBossAttackEvent obj)
    {
        _currentHealth -= damagePerHit;
        _healthBarCanvasController.HealthUpdate(_currentHealth/fullHealth);
        if (_currentHealth <= 0)
        {
            var evt = GameEventsHandler.GameOverEvent;
            evt.IsWin = false;
            EventManager.Broadcast(evt);
        }
    }
}
