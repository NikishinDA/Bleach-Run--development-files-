using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        Vector3 transformPosition = transform.position;
        transformPosition.z = playerTransform.position.z;
        transform.position = transformPosition;
    }
}
