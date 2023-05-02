using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject moveTutorial;
    [SerializeField] private GameObject[] firstLevelTutorials;
    [SerializeField] private GameObject[] secondLevelTutorials;
    private int _tutorialOrder;
    private int _level;
    private void Awake()
    {
        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<TutorialShowEvent>(OnTutorialShow);
        _level = PlayerPrefs.GetInt(PlayerPrefsStrings.Level, 1);
    }

    private void OnDestroy()
    {
        
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<TutorialShowEvent>(OnTutorialShow);
    }

    private void OnTutorialShow(TutorialShowEvent obj)
    {
        if (_level == 1)
        {
            firstLevelTutorials[_tutorialOrder].SetActive(true);
        }
        else if (_level == 2)
        {
            secondLevelTutorials[_tutorialOrder].SetActive(true);
        }

        _tutorialOrder++;
    }

    private void OnGameStart(GameStartEvent obj)
    {
        if (_level == 1)
        {
            moveTutorial.SetActive(true);
        }
    }

}
