using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : MonoBehaviour
{
    private int _playerStage;
    [SerializeField] private float fullHealth = 100f;
    private float _damage;
    private float _currentHealth;
    private HealthBarCanvasController _healthBarCanvasController;
    private bool _isActive;
    private void Awake()
    {
        EventManager.AddListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
        EventManager.AddListener<FinisherPlayerActualHitEvent>(OnActualHit);
        _healthBarCanvasController = GetComponent<HealthBarCanvasController>();
        _currentHealth = fullHealth;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<FinisherPlayerInPosition>(OnPlayerOnPosition);

        EventManager.RemoveListener<FinisherPlayerActualHitEvent>(OnActualHit);
    }

    private void OnPlayerOnPosition(FinisherPlayerInPosition obj)
    {
        _isActive = true;
        if (obj.Rage)
        {
            _playerStage = 3;
        }
        else
        {
            _playerStage = obj.Stage;
        }

        _damage = 5f + _playerStage * 2f;
    }

    private void OnActualHit(FinisherPlayerActualHitEvent obj)
    {
        _currentHealth -= _damage;
        _healthBarCanvasController.HealthUpdate(_currentHealth/fullHealth);
        if (_isActive && _currentHealth <= 0)
        {
            EventManager.Broadcast(GameEventsHandler.BossHealthDepletedEvent);
            _isActive = false;
        }
    }
}
