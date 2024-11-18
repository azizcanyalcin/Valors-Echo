using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour, ISaveManager
{
    public static EndGameManager Instance { get; private set; }
    public int levelUpAmount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void IncreaseLevelUpAmount()
    {
        levelUpAmount += 1;
        //SaveManager.instance.SaveGame();
    }
    public void LoadData(GameData data) 
    {
        StartCoroutine(LoadWithDelay(data));
    }
    public void SaveData(ref GameData data)
    {
        data.levelUpAmount = levelUpAmount;
    }

    IEnumerator LoadWithDelay(GameData data)
    {
        yield return new WaitForSeconds(0.1f);
        //levelUpAmount = data.levelUpAmount;
    }
}
