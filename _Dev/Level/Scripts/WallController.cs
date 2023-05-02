using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WallController : InteractiveObject
{
    [SerializeField] private Rigidbody[] bricks;
    [SerializeField] private float lifeTime;
    private CinemachineImpulseSource _impulseSource;
    [SerializeField] private GameObject _breakEffect;
    protected override void Awake()
    {
        base.Awake();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        DestroyWall();
        _impulseSource.GenerateImpulse();
        Instantiate(_breakEffect, transform.position, Quaternion.identity);
    }

    private void DestroyWall()
    {
        foreach (var brick in bricks)
        {
            brick.useGravity = true;
            brick.isKinematic = false;
            brick.AddExplosionForce(10, transform.position + Vector3.back, 10f, 0, ForceMode.Impulse);
        }

        StartCoroutine(LifeTimer(lifeTime));
    }

    private IEnumerator LifeTimer(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        Destroy(transform.parent.gameObject);
    }
}
