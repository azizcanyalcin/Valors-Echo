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
        levelUpAmount++;
        SaveManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        Invoke(nameof(DelayedLoad), 0.1f);
    }

    private void DelayedLoad(GameData data)
    {
        levelUpAmount = data.levelUpAmount;
    }

    public void SaveData(ref GameData data)
    {
        data.levelUpAmount = levelUpAmount;
    }
}
