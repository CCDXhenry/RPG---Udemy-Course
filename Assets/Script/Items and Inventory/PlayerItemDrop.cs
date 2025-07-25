using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [Range(0f, 1f)]
    [SerializeField] private float playerDropChance = 0.5f; // 玩家掉落物品概率
    public override void GenerateDrops()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> currentEquipment = new List<InventoryItem>(inventory.GetEqipmentItems());
        int playerdropCount = Mathf.Min(dropCount, currentEquipment.Count); // 确保实际掉落数量不超过装备数量
        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0f, 1f) <= playerDropChance)
            {
                // 如果掉落数量为0，则退出循环
                if (dropCount <= 0)
                {
                    break;
                }
                // 如果是装备物品且符合掉落概率，则掉落
                DropItem(item.data);
                // 从装备列表中移除掉落的物品
                inventory.UnequipItem(new KeyValuePair<ItemData_Equipment, InventoryItem>(item.data as ItemData_Equipment, item), true);
                // 减少掉落数量
                dropCount--;
            }
        }

    }

}
