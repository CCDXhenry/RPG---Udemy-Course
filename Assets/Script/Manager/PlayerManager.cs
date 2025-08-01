using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;

    public Player player;

    [SerializeField]
    private int _currentSouls;

    public int currentSouls
    {
        get => _currentSouls;
        set => _currentSouls = Mathf.Clamp(value, 0, 999999);
    }

    public Vector3 currentCheckpointTransfrom;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 销毁当前多余的 GameObject
        }
    }

    public bool HaveEnoughSouls(int amount)
    {
        if (currentSouls >= amount)
        {
            currentSouls -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetCurrentSouls()
    {
        return currentSouls;
    }

    public void LoadGame(GameData _gameData)
    {
        this.currentSouls = _gameData.currentSouls;
        this.currentCheckpointTransfrom = _gameData.currentCheckpointTransfrom;
        if (this.currentCheckpointTransfrom != null)
        {
            player.transform.position = currentCheckpointTransfrom;
        }
        else
        {
            // 如果没有存档点，就回到游戏当前场景的开始位置
            string currentSceneName = SceneManager.GetActiveScene().name;
            player.transform.position = _gameData.sceneSpawnPoint[currentSceneName];
        }
    }

    public void SaveGame(ref GameData _gameData)
    {
        _gameData.currentSouls = this.currentSouls;
        _gameData.currentCheckpointTransfrom = this.currentCheckpointTransfrom;
    }
}
