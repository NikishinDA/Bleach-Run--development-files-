using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private Button reloadButton;
    [SerializeField] private Text coinText;

    private void Awake()
    {
        reloadButton.onClick.AddListener(OnReloadButtonClick);
        coinText.text = VarSaver.MoneyCollected.ToString();
    }

    private void OnEnable()
    {
        AdsModule.instance.ShowInterstitial("LosePopup");
    }

    private void OnReloadButtonClick()
    {        
        

        SceneLoader.ReloadLevel();
    }
}
