using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class ItemObject_Trigger : MonoBehaviour
{

    private ItemObject myItemObject => GetComponentInParent<ItemObject>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {

            if (collision.GetComponent<CharacterStats>().isDead)
                return;
            if (!Inventory.instance.CanAddItem(myItemObject.GetItemData()))
            {
                if (myItemObject.rb.velocity.y == 0)
                    myItemObject.rb.velocity = Vector2.up * 3f; // 如果背包已满，物品将被抛起
                return;
            }

            myItemObject.PickupItem();
        }
    }
}

