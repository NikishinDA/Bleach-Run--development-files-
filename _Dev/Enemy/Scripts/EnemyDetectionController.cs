using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EnemyDetectionController : InteractiveObject
{
    [SerializeField] private int stageKillThreshold;
    [SerializeField] private Animator _animator;
    private CinemachineImpulseSource _impulseSource;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private GameObject corpse;
    protected override void Awake()
    {
        base.Awake();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        int swordStage =  other.transform.parent.GetComponent<PlayerHealthManager>().Stage;
        if (swordStage < stageKillThreshold)
        {
            base.OnTriggerEnter(other);
            //_animator.SetTrigger("Attack");
        }
        else
        {
            Trigger.enabled = false;
            EventManager.Broadcast(GameEventsHandler.PlayerKilledMonsterEvent);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            transform.parent.gameObject.SetActive(false);
            Instantiate(corpse, transform.parent.position, transform.parent.rotation);
        }
        _impulseSource.GenerateImpulse();
    }
}
