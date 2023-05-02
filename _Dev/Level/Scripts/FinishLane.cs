using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLane : MonoBehaviour
{
    private Collider _trigger;
    [SerializeField] private GameObject finisherCamera;
    private void Awake()
    {
        _trigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _trigger.enabled = false;
        var evt = GameEventsHandler.PlayerFinishCrossedEvent;
        evt.PlayerTransform = other.transform.parent;
        EventManager.Broadcast(evt);
        finisherCamera.SetActive(true);
    }
}
