using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAcrossController : EnemyController
{
    [SerializeField] private float border;
    private float _costyl = 0.0001f;
    private void Start()
    {
        _direction = Vector3.right;
    }

    protected override void CalculateDirection()
    {
        if (transform.localPosition.x > border)
        {
            _direction = Vector3.left + Vector3.back * _costyl;

        }
        else if (transform.localPosition.x < - border)
        {
            _direction = Vector3.right + Vector3.back * _costyl;
        }
    }
}
