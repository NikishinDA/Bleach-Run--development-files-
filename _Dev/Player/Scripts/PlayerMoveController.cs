using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMoveController : MonoBehaviour
{
    [Header("X Movement")] [SerializeField]
    private float maxSpeedX;

    [SerializeField] private float movementBoundary;

    [Header("Z Movement")] [SerializeField]
    private float startSpeedZ = 10;

    private float _speedZ;

    private float _newX;
    private float _oldX;
    private Vector3 _newPosition;
    private PlayerInputManager _inputManager;
    private CharacterController _cc;
    private bool _move = false;
    [SerializeField] private float gravityForce;
    [SerializeField] private CinemachineVirtualCamera vCamera;
    private float _additionalMoveX;
    private void Awake()
    {
        _inputManager = GetComponent<PlayerInputManager>();
        _cc = GetComponent<CharacterController>();
        _newPosition = new Vector3();
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<DebugCallEvent>(OnDebugCall);
        EventManager.AddListener<PlayerFinishCrossedEvent>(OnPlayerFinishCross);
        EventManager.AddListener<TutorialToggleEvent>(OnTutorialToggle);
        _speedZ = startSpeedZ;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<DebugCallEvent>(OnDebugCall);
        EventManager.RemoveListener<PlayerFinishCrossedEvent>(OnPlayerFinishCross);
        EventManager.RemoveListener<TutorialToggleEvent>(OnTutorialToggle);

    }

    private void OnTutorialToggle(TutorialToggleEvent obj)
    {
        _move = !obj.Toggle;
    }

    private void OnPlayerFinishCross(PlayerFinishCrossedEvent obj)
    {
        _move = false;
    }

    private void Start()
    {
        _move = false;
    }

    private void OnDebugCall(DebugCallEvent obj)
    {
        _speedZ = obj.Speed;
        maxSpeedX = obj.Strafe;
    }

    private void OnGameOver(GameOverEvent obj)
    {
        _move = false;
        if (!obj.IsWin)
        {
            vCamera.m_Follow = null;
        }
    }

    private void OnGameStart(GameStartEvent obj)
    {
        _move = true;
    }

    private void PlayerPushed()
    {
        _additionalMoveX = - 1 * Mathf.Sign(transform.position.x);
    }
    private void Update()
    {
        if (_move)
        {
            float touchDelta = _inputManager.GetTouchDelta();
            _newX = maxSpeedX * touchDelta  + _additionalMoveX;
            if (Mathf.Abs(transform.position.x + _newX) >= movementBoundary)
            {
                _newX = 0;
            }

            _newPosition.x = _newX;
            _newPosition.y = -gravityForce * Time.deltaTime;
            _newPosition.z = _speedZ * Time.deltaTime;
            _cc.Move(_newPosition);
            if (_additionalMoveX != 0) _additionalMoveX = 0;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerPushed();
        }
    }
}