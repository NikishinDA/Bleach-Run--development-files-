using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCorpseController : MonoBehaviour
{
    
    [SerializeField] private Rigidbody corpseTopPart;
    [SerializeField] private Rigidbody corpseBotPart;
    private void Start()
    {
        corpseBotPart.AddForce(new Vector3(1,1,1) * 10f, ForceMode.Impulse);
        corpseBotPart.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
        corpseTopPart.AddForce(new Vector3(-1,1,1) * 10f, ForceMode.Impulse);
        corpseTopPart.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
        StartCoroutine(LifeTime(5f));
    }

    private IEnumerator LifeTime(float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
