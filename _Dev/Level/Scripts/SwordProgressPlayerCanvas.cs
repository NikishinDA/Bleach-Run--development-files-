using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SwordProgressPlayerCanvas : MonoBehaviour
{
    private Transform _mainCameraTransform;

    [SerializeField] private Slider swordStagePb;
    [SerializeField] private Image swordStageImage;
    [SerializeField] private Sprite[] swordSprites;
    private float _stageProgress;
    private void Awake()
    {
        _mainCameraTransform = Camera.main.transform;
        EventManager.AddListener<PlayerHealthUpdateEvent>(OnHealthUpdate);
        EventManager.AddListener<PlayerStageChangeEvent>(OnStageChange);
    }

    private void OnDestroy()
    {
        
        EventManager.RemoveListener<PlayerHealthUpdateEvent>(OnHealthUpdate);
        EventManager.RemoveListener<PlayerStageChangeEvent>(OnStageChange);
    }
    private void OnStageChange(PlayerStageChangeEvent obj)
    {
        swordStageImage.sprite = swordSprites[obj.Stage];
        swordStagePb.value = 0;
    }

    private void OnHealthUpdate(PlayerHealthUpdateEvent obj)
    {
        _stageProgress = obj.StageHealthNormalized;
    }
    // Update is called once per frame
    void Update()
    {
        swordStagePb.value = Mathf.Lerp(swordStagePb.value, _stageProgress, Time.deltaTime * 10);
        transform.forward = _mainCameraTransform.forward;
    }
}
