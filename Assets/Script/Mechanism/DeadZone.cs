using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterStats character))
        {
            if (!character.isDead)
            {
                character.isDeadZone = true;
                character.Die();
            }
                
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
