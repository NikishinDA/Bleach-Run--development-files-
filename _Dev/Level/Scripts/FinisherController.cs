using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinisherController : MonoBehaviour
{
    private bool _active;
    [SerializeField] private float waitB4Screen;
    [SerializeField] private float inactiveTimer;
    private float _timer;
    private void Awake()
    {
        EventManager.AddListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
        EventManager.AddListener<BossHealthDepletedEvent>(OnBossHealthDepleted);
        EventManager.AddListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        _timer = inactiveTimer;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
        EventManager.RemoveListener<BossHealthDepletedEvent>(OnBossHealthDepleted);
        EventManager.RemoveListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);

    }

    private void OnGameOver(GameOverEvent obj)
    {
        Time.timeScale = 1;
    }

    private void OnFinisherEnd(FinisherEndEvent obj)
    {
        Time.timeScale = 1;
        StartCoroutine(WaitB4Screen(waitB4Screen));
    }

    private void OnBossHealthDepleted(BossHealthDepletedEvent obj)
    {
        Time.timeScale = 0.5f;
        _active = false;
    }

    private void OnPlayerOnPosition(FinisherPlayerInPosition obj)
    {
        _active = true;
    }

    private void Update()
    {
        if (_active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventManager.Broadcast(GameEventsHandler.FinisherPlayerSlashEvent);
                _timer = inactiveTimer;
            }
            if (_timer > 0)
            {
               // _timer -= Time.deltaTime;
            }
            else
            {
                EventManager.Broadcast(GameEventsHandler.FinisherEndEvent);
            }
        }
    }

    private IEnumerator WaitB4Screen(float time)
    {
        yield return new WaitForSeconds(time);
        var evt = GameEventsHandler.GameOverEvent;
        evt.IsWin = true;
        EventManager.Broadcast(evt);
    }

}
