using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cr;
    private Player player;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cr = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir,float _gravity)
    {
        rb.gravityScale = _gravity;
        rb.velocity = _dir;
    }

    
}

