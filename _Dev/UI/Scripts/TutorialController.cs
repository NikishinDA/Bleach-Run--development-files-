using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
        BroadcastToggleEvent(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            BroadcastToggleEvent(false);
        }
    }

    private void BroadcastToggleEvent(bool toggle)
    {
        var evt = GameEventsHandler.TutorialToggleEvent;
        evt.Toggle = toggle;
        EventManager.Broadcast(evt);
    }
}
