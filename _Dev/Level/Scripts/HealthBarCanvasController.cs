using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCanvasController : MonoBehaviour
{
    private Transform _mainCameraTransform;

    [SerializeField] private Slider healthPb;
    private float _actualHealth = 1f;
    private void Awake()
    {
        _mainCameraTransform = Camera.main.transform;
        EventManager.AddListener<PlayerFinishCrossedEvent>(OnFinishCrossed);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<BossHealthDepletedEvent>(OnBossHealthDepleted);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerFinishCrossedEvent>(OnFinishCrossed);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);

        EventManager.RemoveListener<BossHealthDepletedEvent>(OnBossHealthDepleted);
    }

    private void OnBossHealthDepleted(BossHealthDepletedEvent obj)
    {
        
        healthPb.gameObject.SetActive(false);
    }

    private void OnGameOver(GameOverEvent obj)
    {
        healthPb.gameObject.SetActive(false);
    }

    private void OnFinishCrossed(PlayerFinishCrossedEvent obj)
    {
        healthPb.gameObject.SetActive(true);
    }

    public void HealthUpdate(float health)
    {
        _actualHealth = health;
    }
    void Update()
    {
        healthPb.value = Mathf.Lerp(healthPb.value, _actualHealth, Time.deltaTime * 10);
        transform.forward = _mainCameraTransform.forward;
    }
}
