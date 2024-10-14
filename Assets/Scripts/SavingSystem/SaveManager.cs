using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool canEncrypt;
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileManager fileManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            saveManagers = FindAllSaveManagers(); // this was outside of the if
        }
    }
    void Start()
    {
        fileManager = new FileManager(Application.persistentDataPath, fileName, canEncrypt);
        LoadGame();
    }
    [ContextMenu("Fresh Start")]
    public async void DeleteDatabase()
    {
        fileManager = new FileManager(Application.persistentDataPath, fileName, canEncrypt);
        await fileManager.DeleteAsync();
    }
    public void NewGame()
    {
        gameData = new GameData();
    }
    public async void LoadGame()
    {
        gameData = await fileManager.LoadAsync();
        if (gameData == null) NewGame();

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

    }
    public async void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        await fileManager.SaveAsync(gameData);

    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSavedData()
    {
        return fileManager.LoadAsync() != null;
    }
}
