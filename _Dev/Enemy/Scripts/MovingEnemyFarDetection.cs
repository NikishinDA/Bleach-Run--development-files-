using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemyFarDetection : EnemyFarDetection
{
    private EnemyController _enemyController;
    private void Awake()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        _enemyController.Stop();
    }
}
