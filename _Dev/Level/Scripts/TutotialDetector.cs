using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutotialDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventManager.Broadcast(GameEventsHandler.TutorialShowEvent);
    }
}
