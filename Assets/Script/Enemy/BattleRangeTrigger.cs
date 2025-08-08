using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRangeTrigger : MonoBehaviour
{
    public event Action battleRangeonTriggerEnter;
    public event Action battleRangeonTriggerExit;
    public BoxCollider2D boxCr;
    private Coroutine delayeExitCoroutine;
    public float delayeTime = 3f;
    private void Start()
    {
        delayeExitCoroutine = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            if(delayeExitCoroutine != null)
            {
                StopCoroutine(delayeExitCoroutine);
                delayeExitCoroutine = null;
            }
            battleRangeonTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            delayeExitCoroutine = StartCoroutine(DelayeExitCoroutine());
        }
    }

    private IEnumerator DelayeExitCoroutine()
    {
        yield return new WaitForSeconds(delayeTime);
        battleRangeonTriggerExit?.Invoke();
        delayeExitCoroutine = null;
    }

}
