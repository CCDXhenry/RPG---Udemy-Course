using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    public override void Die()
    {

        //装备物品掉落
        PlayerItemDrop playerItemDrop = GetComponent<PlayerItemDrop>();
        if (playerItemDrop != null)
        {
            playerItemDrop.GenerateDrops();
        }

        //灵魂数掉落
        GameManage.instance.lostSouls = PlayerManager.instance.currentSouls;
        //判断是否在死亡区域
        if (isDeadZone)
        {
            GameManage.instance.lostSoulsTransposition = PlayerManager.instance.player.lastGroundCheckTransposition;
        }
        else
        {
            GameManage.instance.lostSoulsTransposition = PlayerManager.instance.player.transform.position;
        }
        PlayerManager.instance.currentSouls = 0;
        AudioManager.instance.PlaySFX(11);
        //AudioManager.instance.PlaySFX(20);
        //触发死亡动画
        base.Die();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        AudioManager.instance.PlaySFX(30);
    }
}
