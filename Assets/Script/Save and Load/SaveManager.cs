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
    [SerializeField] private bool isEcrypt;

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
        //实例化文件处理类
        fileHandler = new FileDataHandler(Application.persistentDataPath, fileName, isEcrypt);
    }

    private void Start()
    {
        saveManagers = FindALLSaveManagers();
        LoadGame();
    }

    public bool HasSaveFile()
    {
        return fileHandler.Load() != null ? true : false;
    }

    [ContextMenu("Delect Save File")]
    public void DeleteSaveFile()
    {
        fileHandler = new FileDataHandler(Application.persistentDataPath, fileName, isEcrypt);
        fileHandler.Delete();
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
