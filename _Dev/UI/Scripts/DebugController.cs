using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    [SerializeField] private InputField speedInput;
    [SerializeField] private InputField strafeInput;
    [SerializeField] private InputField stage1Input;
    [SerializeField] private InputField stage2Input;
    [SerializeField] private InputField stage3Input;
    [SerializeField] private Button startButton;
    private float _speed;
    private float _strafe;
    private float _stage1;
    private float _stage2;
    private float _stage3;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        _speed = PlayerPrefs.GetFloat("DebugSpeed", 10);
        _strafe = PlayerPrefs.GetFloat("DebugStrafe", 10);
        _stage1 = PlayerPrefs.GetFloat("DebugStage1", 100);
        _stage2 = PlayerPrefs.GetFloat("DebugStage2", 250);
        _stage3 = PlayerPrefs.GetFloat("DebugStage3", 400);
        speedInput.text = _speed.ToString();
        strafeInput.text = _strafe.ToString();
        stage1Input.text = _stage1.ToString();
        stage2Input.text = _stage2.ToString();
        stage3Input.text = _stage3.ToString();
    }

    private void OnStartButtonClick()
    {
        var evt = GameEventsHandler.DebugCallEvent;
        Single.TryParse(speedInput.text, out _speed);
        Single.TryParse(strafeInput.text, out _strafe);
        Single.TryParse(stage1Input.text, out _stage1);
        Single.TryParse(stage2Input.text, out _stage2);
        Single.TryParse(stage3Input.text, out _stage3);
        evt.Speed = _speed;
        evt.Strafe = _strafe;
        evt.Stage1 = _stage1;
        evt.Stage2 = _stage2;
        evt.Stage3 = _stage3;
        PlayerPrefs.SetFloat("DebugSpeed", _speed);
        PlayerPrefs.SetFloat("DebugStrafe", _strafe);
        PlayerPrefs.SetFloat("DebugStage1", _stage1);
        PlayerPrefs.SetFloat("DebugStage2", _stage2);
        PlayerPrefs.SetFloat("DebugStage3", _stage3);
        PlayerPrefs.Save();
        EventManager.Broadcast(evt);
    }
}
