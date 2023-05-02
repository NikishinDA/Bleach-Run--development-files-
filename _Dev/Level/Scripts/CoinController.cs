using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private GameObject _pickUpEffect;
    private void OnTriggerEnter(Collider other)
    {
        EventManager.Broadcast(GameEventsHandler.CoinCollectEvent);
        Instantiate(_pickUpEffect, transform.position, Quaternion.identity); 
        //Taptic.Light();

        VibrationTapticFix.Vibrate(50,255);
        Destroy(transform.parent.gameObject);
    }
}
