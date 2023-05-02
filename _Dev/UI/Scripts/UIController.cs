using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject finisherScreen;
    [SerializeField] private GameObject tutorialScreen;
    private int _level;
    private void Awake()
    {
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.AddListener<PlayerFinishCrossedEvent>(OnPlayerFinishCrossed);
        _level = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<FinisherEndEvent>(OnFinisherEnd);
        EventManager.RemoveListener<PlayerFinishCrossedEvent>(OnPlayerFinishCrossed);

    }
    private void OnPlayerFinishCrossed(PlayerFinishCrossedEvent obj)
    {
        finisherScreen.SetActive(true);
    }

    private void OnFinisherEnd(FinisherEndEvent obj)
    {
    }

    private void OnGameOver(GameOverEvent obj)
    {
        overlay.SetActive(false);
        finisherScreen.SetActive(false);
        if (!obj.IsWin)
        {
            loseScreen.SetActive(true);
        }
        else
        {
            winScreen.SetActive(true);
        }
    }

    private void OnGameStart(GameStartEvent obj)
    {
        startScreen.SetActive(false);
        overlay.SetActive(true);
    }

    private void Start()
    {
        startScreen.SetActive(true);
    }
}