using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    [SerializeField] private Text coinText;
    [SerializeField] private Slider swordStagePb;
    [SerializeField] private Image swordCurrentStageImage;
    [SerializeField] private Image swordNextStageImage;
    [SerializeField] private Sprite[] swordCurrentSprites;
    [SerializeField] private Sprite[] swordNextSprites;
    private float _stageProgress;
    [SerializeField] private Slider levelProgressBar;
    private float _levelProgress;
    private float _progressPerLevel;
    [SerializeField] private Text levelText;
    [SerializeField] private Animator moneyTextAnimator;
    private void Awake()
    {
        EventManager.AddListener<CoinCollectEvent>(OnCoinCollected);
        EventManager.AddListener<PlayerHealthUpdateEvent>(OnHealthUpdate);
        EventManager.AddListener<PlayerStageChangeEvent>(OnStageChange);
        EventManager.AddListener<PlayerProgressEvent>(OnPlayerProgress);
        VarSaver.MoneyCollected = 0;
        levelText.text = "LEVEL "+ PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<CoinCollectEvent>(OnCoinCollected);
        EventManager.RemoveListener<PlayerHealthUpdateEvent>(OnHealthUpdate);
        EventManager.RemoveListener<PlayerStageChangeEvent>(OnStageChange);
        EventManager.RemoveListener<PlayerProgressEvent>(OnPlayerProgress);

    }

    private void Start()
    {
        _progressPerLevel = 1f / VarSaver.LevelLength;
    }
    private void OnPlayerProgress(PlayerProgressEvent obj)
    {
        _levelProgress += _progressPerLevel;
    }

    private void OnStageChange(PlayerStageChangeEvent obj)
    {
        if (obj.Stage >= swordCurrentSprites.Length) return;
        swordCurrentStageImage.sprite = swordCurrentSprites[obj.Stage];
        if (obj.Stage < swordNextSprites.Length)
            swordNextStageImage.sprite = swordNextSprites[obj.Stage];
        swordStagePb.value = 0;
    }

    private void OnHealthUpdate(PlayerHealthUpdateEvent obj)
    {
       _stageProgress = obj.StageHealthNormalized;
    }

    private void OnCoinCollected(CoinCollectEvent obj)
    {
        VarSaver.MoneyCollected++;
        coinText.text = VarSaver.MoneyCollected.ToString();
        moneyTextAnimator.SetTrigger("Collect");
    }

    private void Update()
    {
        swordStagePb.value = Mathf.Lerp(swordStagePb.value, _stageProgress, Time.deltaTime * 10);
        levelProgressBar.value = Mathf.Lerp(levelProgressBar.value, _levelProgress, Time.deltaTime);
    }
}
