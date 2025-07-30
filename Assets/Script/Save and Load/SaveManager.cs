using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private GameData gameData;
    private List<ISaveManager> saveManagers;

    [SerializeField] private string fileName;
    private FileDataHandler fileHandler;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        fileHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindALLSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }
    public void LoadGame()
    {
        //从文件中读取数据
        gameData = fileHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No save data found");
            NewGame();
        }
        foreach (var manager in saveManagers)
        {
            manager.LoadGame(gameData);
        }
    }

    public void SaveGame()
    {
        Debug.Log("Game was saved!");
        foreach (var manager in saveManagers)
        {
            manager.SaveGame(ref gameData);
        }

        //将数据保存到文件
        fileHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public List<ISaveManager> FindALLSaveManagers()
    {
        var saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveManager>().ToList();
        return saveManagers;
    }
}
