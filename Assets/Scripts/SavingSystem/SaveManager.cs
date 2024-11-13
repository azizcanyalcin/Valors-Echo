using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private string filePath = "idbfs/azoleite1199";
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
            //DontDestroyOnLoad(gameObject);
            saveManagers = FindAllSaveManagers();
        }
    }

    async void Start()
    {
        fileManager = new FileManager(filePath, fileName, canEncrypt);
        await LoadGame();
    }

    [ContextMenu("Fresh Start")]
    public async void DeleteDatabase()
    {
        await fileManager.DeleteAsync();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public async Task LoadGame()
    {
        gameData = await fileManager.LoadAsync();
        if (gameData == null) NewGame();

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public async Task SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        await fileManager.SaveAsync(gameData);
    }

    private async void OnApplicationQuit()
    {
        await SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public async Task<bool> HasSavedData()
    {
        var data = await fileManager.LoadAsync();
        return data != null;
    }
}

