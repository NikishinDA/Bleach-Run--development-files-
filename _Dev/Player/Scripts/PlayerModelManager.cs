using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] skins;
    private void Awake()
    {
        int skinNumber = PlayerPrefs.GetInt(PlayerPrefsStrings.SkinNumber, 0);
        skins[skinNumber].SetActive(true);
    }
}
