using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRangeTrigger : MonoBehaviour
{
    public event Action battleRangeonTriggerEnter;
    public event Action battleRangeonTriggerExit;
    public BoxCollider2D boxCr;
    private void Start()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            battleRangeonTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            battleRangeonTriggerExit?.Invoke();
        }
    }

}
