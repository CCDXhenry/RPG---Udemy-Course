using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 10f; //�ٶ�    
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cr;
    private Player player;
    private bool canRotate = true;
    private bool isReturning = false;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cr = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir,float _gravity,Player _player)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravity;
        anim.SetBool("Rotation", true);
    }
    public void ReturnSword() 
    { 
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }
    

    public void Update()
    {
        //ʹ���ĳ������ٶȷ���һ��
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //������ӽ���ң����ٽ�����
            if (Vector2.Distance(transform.position,player.transform.position) < 1)
            {
                player.ClearSword();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Sword collided with: " + collision.gameObject.name);
        //������ײ��ֹͣ������ת���ƶ��������丽�ŵ���ײ������
        anim.SetBool("Rotation", false);
        canRotate = false;
        cr.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
    }
  
}

