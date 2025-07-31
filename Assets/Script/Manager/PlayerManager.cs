using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISaveManager
{
    public static PlayerManager instance;
    
    public Player player;

    [SerializeField] private int currency;
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

    public bool HaveEnoughCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetCurrency()
    {
        return currency;
    }

    public void LoadGame(GameData _gameData)
    {
        this.currency = _gameData.currency;
    }

    public void SaveGame(ref GameData _gameData)
    {
        _gameData.currency = this.currency;
    }
}
