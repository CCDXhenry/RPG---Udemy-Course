using Assets.Script.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Singleton instance


    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipmentItems;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_ItemSlot_Equipment[] equipmentItemSlot;
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
                UnequipItem(item);
                AddItem(oldEquipment);
                break;
            }
        }

        equipmentItems.Add(newItem);
        equipmentDictionary[newEquipment] = newItem;
        RemoveItem(_item);

        //UpdateSlotUI();
    }

    private void UnequipItem(KeyValuePair<ItemData_Equipment, InventoryItem> item)
    {
        equipmentItems.Remove(item.Value);
        equipmentDictionary.Remove(item.Key);
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

        for (int i = 0; i < equipmentItemSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentItemSlot[i].equipmentType)
                {
                    
                    equipmentItemSlot[i].UpdateItemSlotUI(item.Value);
                    break;
                }
            }
        }
        
    }

    public void AddItem(ItemData _itemData)
    {
        if (_itemData.itemType == ItemType.Material)
        {
            AddToInventory(_itemData);
        }
        else if (_itemData.itemType == ItemType.Equipment)
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


}
