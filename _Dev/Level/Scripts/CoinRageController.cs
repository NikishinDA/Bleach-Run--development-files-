using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CoinRageController : MonoBehaviour
{
    private bool _rage;
    private Transform _playerTransform;
    private Collider _trigger;
    [SerializeField] private GameObject pickUpEffect;
    private void Awake()
    {
        EventManager.AddListener<PlayerRageEvent>(OnPlayerRage);
        _trigger = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerRageEvent>(OnPlayerRage);
    }
    
    private void OnPlayerRage(PlayerRageEvent obj)
    {
        _rage = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_rage)
        {
            _playerTransform = other.transform;
            StartCoroutine(PlayerAttraction(0.5f));
            _trigger.enabled = false;
        }
    }

    private IEnumerator PlayerAttraction(float time)
    {
        Vector3 startPos = transform.position;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos, _playerTransform.position, t / time);
            yield return null;
        }
        EventManager.Broadcast(GameEventsHandler.CoinCollectEvent);
        Instantiate(pickUpEffect, transform.position, Quaternion.identity);
        //Taptic.Light();
        
        VibrationTapticFix.Vibrate(50,255);
        Destroy(gameObject);
    }
}
