using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum InteractiveType
{
    Monster,
    Wall,
    Fire,
    ArtifactGood,
    ArtifactBad,
    GateGood,
    GateBad
}
public class InteractiveObject : MonoBehaviour
{
    protected Collider Trigger;
    [SerializeField] protected InteractiveType type;
    protected virtual void Awake()
    {
        Trigger = GetComponent<Collider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Trigger.enabled = false;
        var evt = GameEventsHandler.PlayerApplyEffectEvent;
        evt.Type = type;
        EventManager.Broadcast(evt);
    }
}
