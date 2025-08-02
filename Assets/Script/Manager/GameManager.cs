using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour, ISaveManager
{
    public static GameManage instance;

    [Header("Lost Souls")]
    public int lostSouls;
    [SerializeField] private GameObject lostSoulsPrefab;
    public Vector3 lostSoulsTransposition;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadGame(GameData _gameData)
    {
        if (_gameData.lostSouls > 0)
        {
            lostSouls = _gameData.lostSouls;
            lostSoulsTransposition = _gameData.lostSoulsTransposition;
            GameObject lostSoulUI = Instantiate(lostSoulsPrefab, lostSoulsTransposition, Quaternion.identity);
            lostSoulUI.GetComponentInChildren<LostSoulsController>().lostSouls = lostSouls;
        }
    }

    public void SaveGame(ref GameData _gameData)
    {
        _gameData.lostSouls = lostSouls;
        _gameData.lostSoulsTransposition = lostSoulsTransposition;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
