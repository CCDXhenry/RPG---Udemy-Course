using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Singleton instance
    //初始库存
    public ItemData[] initialItems; // 初始物品数据数组
    // 背包
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    // 仓库
    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;
    // 装备
    public List<InventoryItem> equipmentItems;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent; // 用于显示属性的UI容器

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_ItemSlot_Equipment[] equipmentItemSlot;
    private UI_StatSlot[] statSlots;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        stashItems = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        equipmentItems = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<UI_ItemSlot_Equipment>();
        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        // 初始化背包和仓库
        AddinitialItems();
    }

    private void AddinitialItems()
    {
        foreach (ItemData itemData in initialItems)
        {
            AddItem(itemData);
        }
    }

    /// <summary>
    /// 穿戴装备
    /// </summary>
    /// <param name="_item"></param>
    public void EquipItem(ItemData _item)
    {

        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
                UnequipItem(item, false);
                break;
            }
        }

        equipmentItems.Add(newItem);
        equipmentDictionary[newEquipment] = newItem;
        newEquipment.AddModifiers(); // 增加装备属性
        RemoveItem(_item);

        //UpdateSlotUI();
    }

    /// <summary>
    /// 卸下装备
    /// </summary>
    /// <param name="item"></param>
    public void UnequipItem(KeyValuePair<ItemData_Equipment, InventoryItem> item, bool isDrop)
    {
        equipmentItems.Remove(item.Value);
        equipmentDictionary.Remove(item.Key);
        item.Key.RemoveModifiers(); // 移除装备属性
        if (isDrop)
        {
            UpdateSlotUI();
            return;
        }

        AddItem(item.Key);// 将装备放回背包或仓库
    }

    private void UpdateSlotUI()
    {

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            if (i < inventoryItems.Count)
            {
                inventoryItemSlot[i].UpdateItemSlotUI(inventoryItems[i]);
            }
            else
            {
                inventoryItemSlot[i].ClearItemSlotUI(); // Clear slot if no item

            }
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            if (i < stashItems.Count)
            {
                stashItemSlot[i].UpdateItemSlotUI(stashItems[i]);
            }
            else
            {
                stashItemSlot[i].ClearItemSlotUI(); // Clear slot if no item
            }
        }

        // 循环装备槽，更新UI
        for (int i = 0; i < equipmentItemSlot.Length; i++)
        {
            equipmentItemSlot[i].ClearItemSlotUI();
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentItemSlot[i].equipmentType)
                {

                    equipmentItemSlot[i].UpdateItemSlotUI(item.Value);
                    break;
                }
            }
        }

        // 更新属性UI
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _itemData)
    {
        if (!CanAddItem(_itemData))
            return;
        if (_itemData.itemType == ItemType.Equipment)
        {
            AddToInventory(_itemData);
        }
        else if (_itemData.itemType == ItemType.Material)
        {
            AddToStash(_itemData);
        }
        else
        {
            Debug.LogWarning("Item type not supported: " + _itemData.itemType);
        }

        UpdateSlotUI();
    }

    private void AddToStash(ItemData _itemData)
    {
        if (stashDictionary.TryGetValue(_itemData, out InventoryItem existingItem))
        {
            existingItem.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_itemData);
            stashItems.Add(newItem);
            stashDictionary[_itemData] = newItem;
        }
    }

    private void AddToInventory(ItemData _itemData)
    {
        if (inventoryDictionary.TryGetValue(_itemData, out InventoryItem existingItem))
        {
            existingItem.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_itemData);
            inventoryItems.Add(newItem);
            inventoryDictionary[_itemData] = newItem;
        }
    }

    public void RemoveItem(ItemData _itemData)
    {
        if (inventoryDictionary.TryGetValue(_itemData, out InventoryItem value))
        {
            value.RemoveStack();
            if (value.stackSize <= 0)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_itemData);
            }
        }

        if (stashDictionary.TryGetValue(_itemData, out InventoryItem stashValue))
        {
            stashValue.RemoveStack();
            if (stashValue.stackSize <= 0)
            {
                stashItems.Remove(stashValue);
                stashDictionary.Remove(_itemData);
            }
        }
        UpdateSlotUI();
    }

    /// <summary>
    /// 检查是否可以制作物品
    /// </summary>
    /// <param name="_itemToCraft"></param>
    /// <param name="_requiredMaterials"></param>
    /// <returns></returns>
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        // 从仓库中检查所需材料
        foreach (InventoryItem material in _requiredMaterials)
        {
            if (stashDictionary.TryGetValue(material.data, out InventoryItem stashItem))
            {
                if (stashItem.stackSize >= material.stackSize)
                {
                    materialsToRemove.Add(stashItem);
                }
                else
                {
                    return false; // Not enough materials
                }
            }
            else
            {
                return false; // Material not found in stash
            }
        }
        // If we reach here, we have enough materials
        foreach (InventoryItem material in materialsToRemove)
        {
            RemoveItem(material.data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Crafted item: " + _itemToCraft.name);
        return true; // Crafting successful
    }

    public List<InventoryItem> GetEqipmentItems() => equipmentItems;
    public List<InventoryItem> GetInventoryItems() => inventoryItems;
    public List<InventoryItem> GetStashItems() => stashItems;

    public ItemData_Equipment GetEqiupmentEffects(EquipmentType _type)
    {
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                return item.Key;
            }
        }
        return null;
    }
    // 判断是否可以添加物品到背包或仓库
    public bool CanAddItem(ItemData _itemData)
    {
        if (_itemData.itemType == ItemType.Equipment)
        {
            //Debug.Log("inventoryItems.Count: " + inventoryItems.Count + ", inventoryItemSlot.Length: " + inventoryItemSlot.Length);
            return inventoryItems.Count < inventoryItemSlot.Length;
        }
        else if (_itemData.itemType == ItemType.Material)
        {
            return stashItems.Count < stashItemSlot.Length;
        }
        return false; // 不支持的物品类型
    }
}
