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
        Vector2 mousePosition = Input.mousePosition;
        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
        {
            xOffset = -100;
        }
        else
        {
            xOffset = 100;
        }
        if (mousePosition.y > 600)
        {
            yOffset = -100;
        }
        else
        {
            yOffset = 100;
        }

        transform.position = new Vector2(mousePosition.x + xOffset,mousePosition.y + yOffset);

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
