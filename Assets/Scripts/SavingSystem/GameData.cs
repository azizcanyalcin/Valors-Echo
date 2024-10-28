using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipments;
    public SerializableDictionary<string, bool> checkpoints;
    public string lastCheckpoint;

    public int lostCurrencyAmount;
    public float lostCurrencyX;
    public float lostCurrencyY;
    public bool isPlayerDeadOnce;
    public int levelUpAmount;
    public SerializableDictionary<string,float> volumeSettings;
    public GameData()
    {
        lostCurrencyAmount = 0;
        lostCurrencyX = 0;
        lostCurrencyY = 0;
        currency = 0;
        levelUpAmount = 0;
        
        isPlayerDeadOnce = false;

        skillTree = new();
        inventory = new();
        equipments = new();

        checkpoints = new();
        lastCheckpoint = string.Empty;

        volumeSettings = new();
    }
}
