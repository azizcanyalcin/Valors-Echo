using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    private Player player;
    [SerializeField] private Checkpoint[] checkpoints;
    public string lastCheckpointId;

    [Header("Currency Lost")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        player = PlayerManager.instance.player;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData data)
    {
        StartCoroutine(LoadWithDelay(data));
    }

    IEnumerator LoadWithDelay(GameData data)
    {
        yield return new WaitForSeconds(0.1f);
        LoadLostCurrency(data);
        LoadActiveCheckpoints(data);
        LoadPlayerInfo(data);
        
        PlacePlayerToLastCheckpoint();
    }

    public void SaveData(ref GameData data)
    {
        // Save the player's lost currency and last position
        data.lostCurrencyAmount = lostCurrencyAmount;
        data.lostCurrencyX = player.transform.position.x;
        data.lostCurrencyY = player.transform.position.y;

        // Save the last checkpoint ID
        data.lastCheckpoint = lastCheckpointId;
        data.checkpoints.Clear();

        data.isPlayerDeadOnce = player.isPlayerDeadOnce;

        // Save each checkpoint's activation state
        foreach (Checkpoint checkpoint in checkpoints)
        {
            data.checkpoints.Add(checkpoint.id, checkpoint.isActive);
        }
    }

    private void LoadActiveCheckpoints(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair in data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoints();
                }
            }
        }
        lastCheckpointId = data.lastCheckpoint; // Ensure this value is loaded
    }
    
    private void LoadLostCurrency(GameData data)
    {
        lostCurrencyAmount = data.lostCurrencyAmount;
        lostCurrencyX = data.lostCurrencyX;
        lostCurrencyY = data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab,
                new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<CurrencyLostController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private void LoadPlayerInfo(GameData data)
    {
        player.isPlayerDeadOnce = data.isPlayerDeadOnce;
    }

    public string LastCheckpoint()
    {
        // Simply return the last stored checkpoint ID, not the first active one
        return lastCheckpointId;
    }

    private void PlacePlayerToLastCheckpoint()
    {
        // Make sure player is placed at the correct checkpoint based on the last activated one
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.id == lastCheckpointId)
            {
                player.transform.position = checkpoint.transform.position;
                return; // Break out once the correct checkpoint is found
            }
        }
    }

    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
}
