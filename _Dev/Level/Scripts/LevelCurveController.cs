using System;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.CurvedWorld;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelCurveController : MonoBehaviour
{
    private CurvedWorldController _curvedWorldController;

    private void Awake()
    {
        _curvedWorldController = GetComponent<CurvedWorldController>();
        _curvedWorldController.SetBendHorizontalSize(Random.Range(-2f,1f));
    }
}
