using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemTextName;
    [SerializeField] private TextMeshProUGUI itemTextType;
    [SerializeField] private TextMeshProUGUI itemDescription;



    public void ShowToolTip(ItemData_Equipment item)
    {
        itemTextName.text = item.itemName;
        itemTextType.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }

}
