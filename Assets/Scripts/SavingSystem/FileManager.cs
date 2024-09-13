using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

public class FileManager
{
    private string path = "";
    private string fileName = "";
    private bool canEncrypt = false;
    private string decryptKey = "azizcan";
    private static readonly object fileLock = new();

    public FileManager(string path, string fileName, bool canEncrypt)
    {
        this.path = path;
        this.fileName = fileName;
        this.canEncrypt = canEncrypt;
    }

    public async Task SaveAsync(GameData data)
    {
        string fullPath = Path.Combine(path, fileName);
        await Task.Run(() =>
        {
            lock (fileLock)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                    string dataToStore = JsonUtility.ToJson(data, true);

                    if (canEncrypt) dataToStore = EncryptDecrypt(dataToStore);

                    using (FileStream stream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (StreamWriter writer = new(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"File could not be saved! " + fullPath + "\n" + e);
                }
            }
        });
    }

    public async Task<GameData> LoadAsync()
    {
        string fullPath = Path.Combine(path, fileName);
        GameData loadData = null;

        await Task.Run(() =>
        {
            lock (fileLock)
            {
                try
                {
                    string dataToLoad = "";
                    if (File.Exists(fullPath))
                    {
                        using (FileStream stream = new(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (StreamReader reader = new(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }

                        if (canEncrypt) dataToLoad = EncryptDecrypt(dataToLoad);

                        loadData = JsonUtility.FromJson<GameData>(dataToLoad);
                    }
                    else
                    {
                        Debug.LogWarning("File does not exist.");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Data could not be loaded: " + e);
                }
            }
        });

        return loadData;
    }

    public async Task DeleteAsync()
    {
        string fullPath = Path.Combine(path, fileName);
        await Task.Run(() =>
        {
            lock (fileLock)
            {
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("File could not be deleted: " + e);
                    }
                }
            }
        });
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ decryptKey[i % decryptKey.Length]);
        }
        return modifiedData;
    }
}
