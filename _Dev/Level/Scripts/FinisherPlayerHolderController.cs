using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinisherPlayerHolderController : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private float pullTime;

    private void Awake()
    {
        EventManager.AddListener<PlayerFinishCrossedEvent>(OnPlayerFinishCrossed);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerFinishCrossedEvent>(OnPlayerFinishCrossed);
    }

    private void OnPlayerFinishCrossed(PlayerFinishCrossedEvent obj)
    {
        _playerTransform = obj.PlayerTransform;
        StartCoroutine(PlayerPositionPull(pullTime));
    }

    private IEnumerator PlayerPositionPull(float time)
    {
        Vector3 startPos = _playerTransform.position;
        Vector3 endPos = transform.position;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            _playerTransform.position = Vector3.Lerp(startPos,endPos, t/time);
            yield return null;
        }

        _playerTransform.position = endPos;
        var evt = GameEventsHandler.FinisherPlayerInPosition;
        
        PlayerHealthManager healthManager = _playerTransform.GetComponent<PlayerHealthManager>();
        evt.Stage = healthManager.Stage;
        evt.Rage = healthManager.Raged;
        EventManager.Broadcast(GameEventsHandler.FinisherPlayerInPosition);
    }
}
