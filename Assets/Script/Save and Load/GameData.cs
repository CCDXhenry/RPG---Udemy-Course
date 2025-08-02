using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentSouls;//灵魂数
    public int lostSouls;//失去灵魂数
    public Vector3 lostSoulsTransposition;//失去灵魂的位置

    public Vector3 currentCheckpointTransfrom; //当前存档点位置
    public SerializableDictionary<string, Vector3> sceneSpawnPoint;//人物场景出生点

    public SerializableDictionary<string, int> inventory;//背包
    public List<string> equimentId;//装备
    public SerializableDictionary<string, bool> skillTree;//技能树
    public SerializableDictionary<string, bool> checkpoints;//存档点
    public SerializableDictionary<string, float> volumeSlider;//音量
    public GameData()
    {
        this.currentSouls = 0;
        this.lostSouls = 0;
        this.lostSoulsTransposition = new Vector3();
        this.inventory = new SerializableDictionary<string, int>();
        this.equimentId = new List<string>();
        this.skillTree = new SerializableDictionary<string, bool>();
        this.checkpoints = new SerializableDictionary<string, bool>();
        this.currentCheckpointTransfrom = new Vector3();
        this.volumeSlider = new SerializableDictionary<string, float>();
    }

    /// <summary>
    /// 人物场景出生点初始化
    /// </summary>
    private void InitSceneSpawnPoint()
    {
        this.sceneSpawnPoint = new SerializableDictionary<string, Vector3>();
        this.sceneSpawnPoint.Add("MainScene", new Vector3(-6f, 2f, 0));
    }
}
