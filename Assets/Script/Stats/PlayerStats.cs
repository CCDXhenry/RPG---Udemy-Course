using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    protected override void Die()
    {
        base.Die();
        //装备物品掉落
        PlayerItemDrop playerItemDrop = GetComponent<PlayerItemDrop>();
        if (playerItemDrop != null)
        {
            playerItemDrop.GenerateDrops();
        }
    }
}
