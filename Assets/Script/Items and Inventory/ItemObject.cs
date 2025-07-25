using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void SetupVisuals(ItemData _itemData)
    {
        GetComponent<SpriteRenderer>().sprite = _itemData.icon;
        gameObject.name = "ItemData - " + _itemData.itemName;
    }

    public void SetupItem(ItemData _itemData, Vector2 _vector2)
    {
        itemData = _itemData;
        rb.velocity = _vector2;
        SetupVisuals(_itemData);
    }
    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
