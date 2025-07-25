using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDrop : MonoBehaviour
{
    [SerializeField] private ItemData[] possibleDrop;// 掉落物品数据数组
    [SerializeField] protected int dropCount = 3; // 掉落物品数量
    [SerializeField] private List<ItemData> dropList = new List<ItemData>(); // 已掉落的物品列表

    [SerializeField] protected GameObject dropPrefab; // 物品掉落预制体

    public virtual void GenerateDrops()
    {
        if (possibleDrop.Length == 0)
        {
            Debug.LogWarning("No possible drops set.");
            return;
        }
        dropList.Clear(); // 清空已掉落物品列表

        // 根据掉落概率选择掉落物品
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0f, 1f) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        // 如果掉落物品数量大于0，则随机选择一个物品作为掉落物
        int actualDropCount = Mathf.Min(dropCount, dropList.Count);// 确保实际掉落数量不超过可掉落物品数量
        for (int i = 0; i < actualDropCount; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count)];

            dropList.Remove(randomItem); // 从列表中移除已选择的物品
            DropItem(randomItem); // 掉落物品
        }
    }


    protected void DropItem(ItemData _itemData)
    {
        Debug.Log($"Dropping item: {_itemData.name}");
        if (dropPrefab != null && _itemData != null)
        {
            GameObject dropInstance = Instantiate(dropPrefab, transform.position, Quaternion.identity);
            ItemObject itemObject = dropInstance.GetComponent<ItemObject>();
            if (itemObject != null)
            {
                itemObject.SetupItem(_itemData, new Vector2(Random.Range(-5, 5), Random.Range(5, 10))); // 设置掉落物品数据
            }
        }
        else
        {

            Debug.LogWarning("DropPrefab or ItemData is not set.");
        }
    }
}

