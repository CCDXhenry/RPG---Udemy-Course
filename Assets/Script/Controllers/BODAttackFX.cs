using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODAttackFX : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    public Rigidbody2D rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        CloseCollider();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            AudioManager.instance.PlaySFX(1);
            var enemy = GameObject.Find("Enemy_BringerOfDeath").GetComponent<EnemyStats>();
            enemy.DoDamage(player.stats);
        }
    }
    public void OpenCollider()
    {
        cd.enabled = true;
    }
    public void CloseCollider()
    {
        cd.enabled = false;
    }
    public void FinishAnimation()
    {
        //triggerCalled = true;
        
        Destroy(gameObject);
        
    }
}
