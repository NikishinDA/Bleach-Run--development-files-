using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Text moneyText;
    [SerializeField] private Slider skinProgressBar;
    [SerializeField] private float progressPerLevel = 0.4f;
    private int _skinNumber;
    [SerializeField] private Image pbBackground;
    [SerializeField] private Image pbImage;
    [SerializeField] private Sprite[] skinsBackgrounds;
    [SerializeField] private Sprite[] skinsSprites;

    private void Awake()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
        _skinNumber = PlayerPrefs.GetInt(PlayerPrefsStrings.SkinNumber, 0);
        pbBackground.sprite = skinsBackgrounds[_skinNumber];
        pbImage.sprite = skinsSprites[_skinNumber];
    }

    private void Start()
    {
        moneyText.text = VarSaver.MoneyCollected + " x " + VarSaver.Multiplier.ToString("F1") + " = " + VarSaver.MoneyCollected;
        StartCoroutine(SkinProgress(1f));
        StartCoroutine(MoneyCounter());
    }

    private void OnEnable()
    {
        AdsModule.instance.ShowInterstitial("WinPopup");
    }

    private IEnumerator MoneyCounter()
    {
        yield return new WaitForSeconds(1.5f);
        int money = VarSaver.MoneyCollected;
        int endMoney = (int) (money * VarSaver.Multiplier);
        while (money < endMoney)
        {
            money++;
            moneyText.text = VarSaver.MoneyCollected + " x " + VarSaver.Multiplier.ToString("F1") + " = " + money;
            yield return null;
        }

        
        moneyText.text = VarSaver.MoneyCollected + " x " + VarSaver.Multiplier.ToString("F1") + " = " + endMoney;

    }

    private  IEnumerator SkinProgress(float time)
    {
        float weaponProgress = PlayerPrefs.GetFloat(PlayerPrefsStrings.SkinProgress, 0.01f);
        float endProgress = weaponProgress + progressPerLevel;
        if (endProgress >= 1)
        {
            endProgress = 1;
            PlayerPrefs.SetFloat(PlayerPrefsStrings.SkinProgress, 0);
            _skinNumber = (_skinNumber + 1) % 3;
            PlayerPrefs.SetInt(PlayerPrefsStrings.SkinNumber, _skinNumber);
        }
        else
        {
            PlayerPrefs.SetFloat(PlayerPrefsStrings.SkinProgress, endProgress);
        }
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            skinProgressBar.value = Mathf.Lerp(weaponProgress, endProgress, t / time);
            yield return null;
        }
        
    }
    private void OnNextButtonClick()
    { 
        int totalMoney = PlayerPrefs.GetInt(PlayerPrefsStrings.MoneyTotal, 0);
        totalMoney += (int) (VarSaver.MoneyCollected * VarSaver.Multiplier);
        PlayerPrefs.SetInt(PlayerPrefsStrings.MoneyTotal, totalMoney);
        PlayerPrefs.Save();
        SceneLoader.LoadNextLevel();
    }
}
