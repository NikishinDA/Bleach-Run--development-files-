using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverGatesController : MonoBehaviour
{
    [SerializeField] private Collider leftGateTrigger;
    [SerializeField] private Collider rightGateTrigger;

    public void GatePassed()
    {
        leftGateTrigger.enabled = false;
        rightGateTrigger.enabled = false;
    }
}
