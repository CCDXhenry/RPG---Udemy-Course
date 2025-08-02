using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    private UI_Controller ui;
    public InventoryItem item;
    private void Start()
    {
        ui = GetComponentInParent<UI_Controller>();
    }
    public virtual void UpdateItemSlotUI(InventoryItem _newItem)
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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                Inventory.instance.EquipItem(item.data, true);

            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                Inventory.instance.RemoveItem(item.data);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            //Debug.Log("---------:" + item.data.itemName);
            ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            ui.itemToolTip.HideToolTip();
        }
    }
}
