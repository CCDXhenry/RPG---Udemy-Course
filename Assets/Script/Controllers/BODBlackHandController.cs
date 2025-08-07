using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODBlackHandController : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D boxCollider;
    //private bool triggerCalled = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        CloseCollider();
        AudioManager.instance.PlaySFX(26);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            AudioManager.instance.PlaySFX(27);
            var enemy = GameObject.Find("Enemy_BringerOfDeath").GetComponent<EnemyStats>();
            enemy.DoDamage(player.stats);
        }
    }


    public void OpenCollider()
    {
        boxCollider.enabled = true;
    }
    public void CloseCollider()
    {
        boxCollider.enabled = false;
    }

    public void FinishAnimation()
    {
        //triggerCalled = true;
        GameObject parentObject = transform.parent.gameObject;
        Destroy(gameObject);
        if (parentObject != null)
        {
            Destroy(parentObject);
        }
    }

}
