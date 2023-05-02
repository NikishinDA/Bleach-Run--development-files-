using System;
using System.Collections;
using System.Collections.Generic;
using HighlightPlus;
using UnityEngine;

public class SwordModelController : MonoBehaviour
{
    private Vector3 _startScale;
    private float _intermediateHealth;
    //[SerializeField] private Transform swordTransform;

    private float _stageHealth = 1;

    private float StageHealth
    {
        get => _stageHealth;
        set
        {
            _stageHealth = value; 
            BroadcastUpdateEvent();
        }
    }
    private float _ratio;
    [SerializeField] private float maxScale = 3f;
    [SerializeField] private GameObject[] swordStages;
    [SerializeField] private GameObject rageStage;
    [SerializeField] private GameObject rageSecondary;
    private int _previousStage = 0;
    private bool _rage;
    private HighlightEffect _highlightEffect;
    private IEnumerator _effectCor;
    [SerializeField] private ParticleSystem _swordChangeEffect;
    private void Awake()
    {
        _startScale = Vector3.one;
        _intermediateHealth = StageHealth;
        EventManager.AddListener<PlayerStageHealthRecalculateRatioEvent>(OnRecalculateRatio);
        EventManager.AddListener<PlayerStageHealthUpdateEvent>(OnHealthUpdate);
        EventManager.AddListener<PlayerStageChangeEvent>(OnSwordStageChange);
        EventManager.AddListener<PlayerRageEvent>(OnPlayerRage);
        
        _highlightEffect = GetComponent<HighlightEffect>();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerStageHealthRecalculateRatioEvent>(OnRecalculateRatio);
        EventManager.RemoveListener<PlayerStageHealthUpdateEvent>(OnHealthUpdate);
        EventManager.RemoveListener<PlayerStageChangeEvent>(OnSwordStageChange);
        EventManager.RemoveListener<PlayerRageEvent>(OnPlayerRage);

    }

    private void OnPlayerRage(PlayerRageEvent obj)
    {
        swordStages[_previousStage].SetActive(false);
        rageStage.SetActive(true);
        rageSecondary.SetActive(true);
        _rage = true;
        _previousStage = swordStages.Length;
    }

    private void OnRecalculateRatio(PlayerStageHealthRecalculateRatioEvent obj)
    {
        CalculateRatio(obj.A, obj.B);
    }

    private void OnHealthUpdate(PlayerStageHealthUpdateEvent obj)
    {
        UpdateStageHealth(obj.Difference);
    }

    private void OnSwordStageChange(PlayerStageChangeEvent obj)
    {
        UpdateModel(obj.Stage);
        SetStageHealth(obj.SetStageHealth);
    }

    private void Update()
    {
        _intermediateHealth = Mathf.Lerp(_intermediateHealth, StageHealth, Time.deltaTime);
            transform.localScale = _startScale * (_intermediateHealth);
    }

    private void CalculateRatio(float a, float b)
    {
        _ratio =  (maxScale - 1) / (a - b);
    }
    private void UpdateStageHealth(float diff)
    {
        StageHealth = Mathf.Clamp(StageHealth + diff * _ratio, 1, maxScale);
    }

    private void SetStageHealth(float value)
    {
        StageHealth = value == -1 ? maxScale : value;
    }
    private void UpdateModel(int num)
    {
        if (num > -1 && num < swordStages.Length)
        {
            swordStages[num].SetActive(true);
            if (_previousStage > -1 && _previousStage < swordStages.Length)
                swordStages[_previousStage].SetActive(false);
            _previousStage = num;
            if (_rage)
            {
                rageStage.SetActive(false);
                rageSecondary.SetActive(false);
                _rage = false;
            }
        }
        if (_effectCor != null) StopCoroutine(_effectCor);
        StartCoroutine(_effectCor = EffectTimer(2f));
        _swordChangeEffect.Play();
    }

    private void BroadcastUpdateEvent()
    {
        var evt = GameEventsHandler.PlayerHealthUpdateEvent;
        evt.StageHealth = StageHealth;
        evt.StageHealthNormalized = (StageHealth - 1) / (maxScale - 1);
        EventManager.Broadcast(evt);
    }

    private IEnumerator EffectTimer(float time)
    {
        _highlightEffect.enabled = true;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            yield return null;
        }
        _highlightEffect.enabled = false;
    }
}
