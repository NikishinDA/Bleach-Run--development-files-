using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public int Stage { get; private set; }

    private float _currentHealth = 1f;
    [SerializeField] private float monsterDamage;
    [SerializeField] private float wallDamage;
    [SerializeField] private float fireDamage;
    [SerializeField] private float artifactEffect;
    [SerializeField] private float gateEffect;
    [SerializeField] private float monsterKillEffect = 10f;
    [SerializeField] private float maxValue;
    [SerializeField] private float[] stagesThreshold;
    private bool _raged;

    public bool Raged => _raged;

    private SwordModelController _swordModelController;
    [Header("Finisher")] [SerializeField] private float finisherHitValue;
    private void Awake()
    {
        EventManager.AddListener<PlayerApplyEffectEvent>(OnApplyEffect);
        EventManager.AddListener<PlayerKilledMonsterEvent>(OnKilledMonster);
        //_startScale = swordTransform.localScale;
        //_intermediateHealth = _currentHealth;
        //_swordModelController = GetComponent<SwordModelController>();
        //_swordModelController.CalculateRatio(stagesThreshold[0],0);
        EventManager.AddListener<DebugCallEvent>(OnDebugCall);
        //EventManager.AddListener<FinisherPlayerActualHitEvent>(OnFinisherHit);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<PlayerApplyEffectEvent>(OnApplyEffect);
        EventManager.RemoveListener<PlayerKilledMonsterEvent>(OnKilledMonster);
        EventManager.RemoveListener<DebugCallEvent>(OnDebugCall);

        //EventManager.RemoveListener<FinisherPlayerActualHitEvent>(OnFinisherHit);
    }

   /* private void OnFinisherHit(FinisherPlayerActualHitEvent obj)
    {
        FinisherHealthDeplete(finisherHitValue);
    }*/

    private void Start()
    {
        BroadCastRecalculateRatioEvent(stagesThreshold[0],0);
    }

    private void OnDebugCall(DebugCallEvent obj)
    {
        stagesThreshold[0] = obj.Stage1;
        stagesThreshold[1] = obj.Stage2;
        stagesThreshold[2] = obj.Stage3;
    }

    private void OnKilledMonster(PlayerKilledMonsterEvent obj)
    {
        AdjustHealth(monsterKillEffect);
        Taptic.Success();
    }

    private void AdjustHealth(float diff)
    {
        if (_raged) return;
        _currentHealth = Mathf.Clamp(_currentHealth + diff, 1, maxValue);
        BroadcastUpdateHealthEvent(diff);//_swordModelController.UpdateStageHealth(diff);
        AdjustStage();
    }

    /*private void FinisherHealthDeplete(float value)
    {
        value = -Mathf.Abs(value);
        if (_currentHealth > 0)
        {
            _currentHealth += value;
            AdjustStage();
            BroadcastUpdateHealthEvent(value);
        }
        else
        {
            EventManager.Broadcast(GameEventsHandler.FinisherEndEvent);
        }
            
    }*/
    private void AdjustStage()
    {
        if (Stage < 3 && _currentHealth > stagesThreshold[Stage])
        {
            _currentHealth = stagesThreshold[Stage] + 1;
            if (Stage < 2)
                BroadCastRecalculateRatioEvent(stagesThreshold[Stage + 1],
                    stagesThreshold[
                        Stage]); //_swordModelController.CalculateRatio(stagesThreshold[Stage + 1 ],stagesThreshold[Stage]);
            Stage++;
            _raged = Stage == 3;
            if (_raged)
            {
                EventManager.Broadcast(GameEventsHandler.PlayerRageEvent);
            }

            //_swordModelController.SetStageHealth(1);
            //_swordModelController.UpdateModel(Stage);
            BroadcastStageChangeEvent(1);
        }
        else if (Stage > 0 && _currentHealth < stagesThreshold[Stage - 1])
        {
            Stage--;
            _currentHealth = stagesThreshold[Stage];
            if (Stage > 0)
                BroadCastRecalculateRatioEvent(stagesThreshold[Stage],
                    stagesThreshold[
                        Stage - 1]); //_swordModelController.CalculateRatio(stagesThreshold[Stage],stagesThreshold[Stage - 1]);

            else
                BroadCastRecalculateRatioEvent(stagesThreshold[Stage],
                    0); //_swordModelController.CalculateRatio(stagesThreshold[Stage],0);
            //_swordModelController.SetStageHealth(-1);
            //_swordModelController.UpdateModel(Stage);
            BroadcastStageChangeEvent(-1);
        }
    }

    private void BroadcastUpdateHealthEvent(float diff)
    {
        var evt = GameEventsHandler.PlayerStageHealthUpdateEvent;
        evt.Difference = diff;
        EventManager.Broadcast(evt);

    }

    private void BroadCastRecalculateRatioEvent(float a, float b)
    {
        var evt = GameEventsHandler.PlayerStageHealthRecalculateRatioEvent;
        evt.A = a;
        evt.B = b;
        EventManager.Broadcast(evt);
    }
    private void OnApplyEffect(PlayerApplyEffectEvent obj)
    {
        switch (obj.Type)
        {
            case InteractiveType.Monster:
                //CheckDeath(-monsterDamage);
                //AdjustHealth(-monsterDamage);
                var evt = GameEventsHandler.GameOverEvent;
                evt.IsWin = false;
                EventManager.Broadcast(evt);
                //Taptic.Failure();
                //AndroidTaptic.AndroidVibrate(200, 255);
                VibrationTapticFix.Vibrate(200,255);
                break;
            case InteractiveType.Wall:
                CheckDeath(-wallDamage);
                AdjustHealth(-wallDamage);
                //Taptic.Heavy(); 
                
                VibrationTapticFix.Vibrate(200,255);

                break;
            case InteractiveType.Fire:
                CheckDeath(-fireDamage);
                AdjustHealth(-fireDamage);
                //Taptic.Heavy(); 
                
                VibrationTapticFix.Vibrate(200,255);

                break;
            case InteractiveType.ArtifactGood:
                AdjustHealth(artifactEffect);
                //Taptic.Light();
                
                VibrationTapticFix.Vibrate(50,255);

                break;
            case InteractiveType.ArtifactBad:
                AdjustHealth(-artifactEffect);
                //Taptic.Medium();
                
                
                VibrationTapticFix.Vibrate(50,255);
                break;
            case InteractiveType.GateGood:
                AdjustHealth(gateEffect);
                //Taptic.Success();
                
                VibrationTapticFix.Vibrate(200,255);
                break;
            case InteractiveType.GateBad:
                AdjustHealth(-gateEffect);
                //Taptic.Failure();
                
                VibrationTapticFix.Vibrate(200,255);
                break;
        }
    }

    private void CheckDeath(float diff)
    {
        if (_currentHealth + diff <= 0)
        {
            var evt = GameEventsHandler.GameOverEvent;
            evt.IsWin = false;
            EventManager.Broadcast(evt);
        }
    }

    private void BroadcastStageChangeEvent(float stageHealth)
    {
        var evt = GameEventsHandler.PlayerStageChangeEvent;
        evt.Stage = Stage;
        evt.SetStageHealth = stageHealth;
        EventManager.Broadcast(evt);
    }
}