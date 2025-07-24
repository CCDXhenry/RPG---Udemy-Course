using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler
{
    
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;
    public void UpdateItemSlotUI(InventoryItem _newItem)
    {
        item = _newItem;
        if (item != null)
        {
            itemImage.color = Color.white;
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }

        }
    }

    public void ClearItemSlotUI()
    {
        item = null;
        itemImage.color = Color.clear;
        itemImage.sprite = null;
        itemText.text = "";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                Inventory.instance.EquipItem(item.data);
                
            }
        }
    }
}
