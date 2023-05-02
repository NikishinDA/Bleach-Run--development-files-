using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactController : InteractiveObject
{
    [SerializeField] private GameObject pickUpEffect;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Instantiate(pickUpEffect, transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }
}
