using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UI_ItemSlot_Equipment : UI_ItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + equipmentType;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                Inventory.instance.UnequipItem(new KeyValuePair<ItemData_Equipment, InventoryItem>(item.data as ItemData_Equipment, item), false);
            }
        }
    }
}

