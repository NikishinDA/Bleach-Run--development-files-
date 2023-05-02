using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHolderController : MonoBehaviour
{
    private void OnEnable()
    {
        var evt = GameEventsHandler.SwordHolderEnableEvent;
        evt.SwordHolderTransform = transform;
        EventManager.Broadcast(evt);
    }
}
