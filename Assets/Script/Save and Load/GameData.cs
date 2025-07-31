using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;//金币数
    public SerializableDictionary<string, int> inventory;//背包
    public List<string> equimentId;//装备
    public SerializableDictionary<string, bool> skillTree;//技能树
    public GameData()
    {
        this.currency = 0;
        this.inventory = new SerializableDictionary<string, int>();
        this.equimentId = new List<string>();
        this.skillTree = new SerializableDictionary<string, bool>();
    }
}
