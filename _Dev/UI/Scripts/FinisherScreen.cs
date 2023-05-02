using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinisherScreen : MonoBehaviour
{
    [SerializeField] private Text textPrefab;
    [SerializeField] private Transform textSpawn;
    private bool _onLeft;
    private int _num;
    private int _playerStage;
    private bool _isActive;
    private readonly string[] _multipliers =
    {
        "x1,0", "x1,2", "x1,4", "x1,6", "x1,8",
        "x2,0", "x2,2", "x2,4", "x2,6", "x2,8",
        "x3,0", "x3,2", "x3,4", "x3,6", "x3,8",
        "x4,0", "x4,2", "x4,4", "x4,6", "x4,8",
        "x5,0"
    };

    private float _multiplierValue = 1f;
    private void Awake()
    {
        EventManager.AddListener<FinisherPlayerActualHitEvent>(OnHitEvent);
        EventManager.AddListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.AddListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<FinisherPlayerActualHitEvent>(OnHitEvent);
        EventManager.RemoveListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.RemoveListener<FinisherPlayerInPosition>(OnPlayerOnPosition);
    }

    private void OnPlayerOnPosition(FinisherPlayerInPosition obj)
    {
        if (obj.Rage)
        {
            _playerStage = 3;
        }
        else
        {
            _playerStage = obj.Stage;
        }

        _isActive = true;
    }

    private void OnFinisherEnd(FinisherEndEvent obj)
    {
        VarSaver.Multiplier = _multiplierValue;
        _isActive = false;
    }

    private void OnHitEvent(FinisherPlayerActualHitEvent obj)
    {
        if (!_isActive) return;
        Text multiText = Instantiate(textPrefab, textSpawn);
        _multiplierValue = Mathf.Clamp(_multiplierValue + 0.125f * _playerStage + 0.075f, 1f, 5f);
        multiText.text = "x"+ _multiplierValue.ToString("F1");
        //if (_num < _multipliers.Length - 1)
         //   _num++;
        if (_onLeft)
        {
            multiText.GetComponent<Animator>().SetTrigger("Left");
        }
        else
        {
            multiText.GetComponent<Animator>().SetTrigger("Right");
        }

        _onLeft = !_onLeft;
    }
}