using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoulsController : MonoBehaviour
{
    public int lostSouls;
    public event Action OnLostSoulsTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            //玩家捡魂
            PlayerManager.instance.currentSouls += lostSouls;
            GameManage.instance.lostSouls = 0;
            GameManage.instance.lostSoulsTransposition = Vector3.zero;
            Destroy(gameObject);
            OnLostSoulsTriggered?.Invoke();
        }
    }
}
