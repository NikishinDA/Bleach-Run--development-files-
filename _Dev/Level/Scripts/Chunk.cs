using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Chunk : MonoBehaviour
{
    public Transform begin;
    public Transform end;
    [SerializeField] private float width;
    [SerializeField] private float length;

    [SerializeField] private Plain plainObject;
    [SerializeField] private Plain[] plains;
    [SerializeField] private GameObject[] chunkObjects;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform plainSpawn;

    //private Renderer _plainRenderer;
    public void InitializeChunk(ChunkTemplate template)
    {
        Vector3 objectPos = new Vector3();
        plainObject = Instantiate(plains[(int) template.chunkType], plainSpawn);
        begin = plainObject.begin;
        end = plainObject.end;
        length = end.position.z - begin.position.z;
        //_plainRenderer = plainObject.GetComponent<Renderer>();
        foreach (var levelObject in template.objects)
        {
            objectPos.x = levelObject.position.x * width;
            objectPos.z = levelObject.position.y * length;
            objectPos.y = 10f;
            Transform go = Instantiate(chunkObjects[(int) levelObject.type], transform).transform;
            float originY = go.position.y;
            go.localPosition = objectPos;
            RaycastHit hit;
            Color rayColor = Color.red;
            float dist = 20f;
            if (Physics.Raycast(
                go.position,
                Vector3.down,
                out hit,
                20f,
                layerMask))
            {
                objectPos.y = originY + hit.point.y;

                go.localPosition = objectPos;
                rayColor = Color.green;
                dist = hit.distance;
            }

            Debug.DrawRay(go.position, Vector3.down * dist, rayColor, 60f);
        }
    }
}