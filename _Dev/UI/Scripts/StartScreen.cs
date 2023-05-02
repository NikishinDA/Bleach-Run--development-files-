using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Text moneyCounter;
    
    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        moneyCounter.text = PlayerPrefs.GetInt(PlayerPrefsStrings.MoneyTotal, 0).ToString();
    }

    private void OnStartButtonClick()
    {
        EventManager.Broadcast(GameEventsHandler.GameStartEvent);
    }
}
