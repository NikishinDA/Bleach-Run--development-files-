using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarDetection : MonoBehaviour
{
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Animator enemyAnimator;
    protected virtual void OnTriggerEnter(Collider other)
    {
        enemyAnimator.SetTrigger("Attack");
    }

    private void OnTriggerStay(Collider other)
    {
        enemyTransform.LookAt(other.transform.parent);
    }
}
