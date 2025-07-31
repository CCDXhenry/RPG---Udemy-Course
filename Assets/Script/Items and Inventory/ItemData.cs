using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
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
    public string itemId;
    public Sprite icon;
    public StringBuilder description = new StringBuilder();

    [Range(0, 1)]
    public float dropChance = 1f; // 掉落几率，默认100%

    public virtual string GetDescription() { return description.ToString(); }

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
