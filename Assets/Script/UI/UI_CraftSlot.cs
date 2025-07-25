using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class UI_CraftSlot : UI_ItemSlot
{
    private void OnValidate()
    {
        gameObject.name = "Craft - " + item.data.itemName;
    }
    private void OnEnable()
    {
        UpdateItemSlotUI(item);
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;
        if (!Inventory.instance.CanCraft(craftData, craftData.craftingMaterials))
        {
            Debug.LogWarning("Not enough materials to craft: " + craftData.itemName);
        }
    }
}

