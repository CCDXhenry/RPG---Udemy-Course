using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour ,ISaveManager
{
    [SerializeField] private Animator anim;
    public string checkpointId;
    public bool isActive;
    private void Start()
    {
        //anim = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            TriggerCheckpoint(player);
        }

    }

    private void TriggerCheckpoint(Player player)
    {
        isActive = true;
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("fadeIn"))
            anim.SetTrigger("fadeIn");
        // 存储当前存档点位置
        PlayerManager.instance.currentCheckpointTransfrom = anim.gameObject.transform.position;
        // 保存游戏
        SaveManager.instance.SaveGame();
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        checkpointId = System.Guid.NewGuid().ToString();
    }

    public void LoadGame(GameData _gameData)
    {
        isActive = _gameData.checkpoints.TryGetValue(checkpointId,out bool value) ? value : false;
        if (isActive)
        {
            anim.SetTrigger("fadeIn");
        }
    }

    public void SaveGame(ref GameData _gameData)
    {
        _gameData.checkpoints[checkpointId] = isActive;
    }
}
