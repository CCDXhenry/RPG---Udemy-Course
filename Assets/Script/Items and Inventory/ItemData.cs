using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment,
    //Consumable,
    //QuestItem,
    //Miscellaneous
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0,1)]
    public float dropChance = 1f; // 掉落几率，默认100%
}
