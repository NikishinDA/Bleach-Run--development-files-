using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private float _playTimer;
    private void Awake()
    {
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        GameAnalytics.Initialize();
        
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
    }

    private void OnGameStart(GameStartEvent obj)
    {
        int level = PlayerPrefs.GetInt("Level", 1);
        GameAnalytics.NewProgressionEvent (
            GAProgressionStatus.Start,
            "Level_" + level);
        
        string level_id = "level_" + level;
        bool measureFPS = true;
        HoopslyIntegration.Instance.RaiseLevelStartEvent(level_id, measureFPS, "", "");
        
        StartCoroutine(Timer());
    }

    private void OnGameOver(GameOverEvent obj)
    {
        int level = PlayerPrefs.GetInt("Level", 1);
        var status = obj.IsWin? GAProgressionStatus.Complete : GAProgressionStatus.Fail;
        GameAnalytics.NewProgressionEvent(
            status,
            "Level_" + level,
            "PlayTime_" + Mathf.RoundToInt(_playTimer));
        
        LevelFinishedResult finishedResult = obj.IsWin? LevelFinishedResult.win : LevelFinishedResult.lose;
        string level_id = "level_" + level;
        HoopslyIntegration.Instance.RaiseLevelFinishedEvent(level_id, finishedResult, "", "", "");
    }
    private IEnumerator Timer()
    {
        for (;;)
        {
            _playTimer += Time.deltaTime;
            yield return null;
        }
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            SceneLoader.ReloadLevel();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            SceneLoader.LoadNextLevel();
        }
    }
    
#endif
}
