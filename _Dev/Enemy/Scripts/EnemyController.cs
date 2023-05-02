using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] private float rotationSpeed;
    protected Vector3 _direction;
    private CharacterController _cc;
    private bool _move = true;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!_move) return;
        CalculateDirection();
        //transform.Translate(transform.forward * (speed * Time.deltaTime));
        Vector3 localPosition = transform.localPosition;
        transform.localPosition = Vector3.MoveTowards(localPosition, localPosition + _direction, speed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(_direction), Time.deltaTime * rotationSpeed);
    }

    protected abstract void CalculateDirection();

    public void Stop()
    {
        _move = false;
    }
}