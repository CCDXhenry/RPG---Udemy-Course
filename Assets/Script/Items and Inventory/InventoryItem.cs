using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData itemData)
    {
        this.data = itemData;
        AddStack();
    }

    public void AddStack()
    {
        stackSize++;
    }

    public void RemoveStack()
    {
        if (stackSize > 0)
        {
            stackSize--;
        }
        else
        {
            Debug.LogWarning("Cannot remove stack, stack size is already zero.");
        }
    }
}
