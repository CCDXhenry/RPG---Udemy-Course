using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] float jumpForce = 200f;
    [SerializeField] float jumpCounts = 2;
    float jumpCount = 0;
    private Rigidbody2D rb;
    private int faceDirection = 1;
    [SerializeField] GameObject headPos;
    [SerializeField] Vector3 repellingForce;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && jumpCount < jumpCounts)
        {
            var mousePos = Input.mousePosition;
            var playerPos = Camera.main.WorldToScreenPoint(transform.position);
            if (faceDirection > 0 && mousePos.x < playerPos.x)
            {
                faceDirection = -faceDirection;
                transform.Rotate(0, 180, 0); 
            }
            else if (faceDirection < 0 && mousePos.x >= playerPos.x)
            {
                faceDirection = -faceDirection;
                transform.Rotate(0, 180, 0);
            }
            jumpCount++;
            rb.velocity = new Vector2(0,jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Stone"))
        {
            jumpCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out Weapon weapon))
        {
            if (jumpCount == 0)
            {
                Die();
                Debug.Log("人物未起跳击打武器,人物死亡!");
                return;
            }
            if (weapon.type == WeaponType.Dart)
            {
                Die();
                Debug.Log("被飞镖打中,人物死亡!");
                return;
            }
            if (weapon.transform.position.y >= headPos.transform.position.y)
            {
                Die();
                Debug.Log("武器超过任务头顶,人物死亡!");
                return;
            }
            int weaponDir = weapon.transform.position.x >= transform.position.x ? 1 : -1;
            if(weaponDir != faceDirection)
            {
                Die();
                Debug.Log("武器击中人物背面,人物死亡!");
                return;
            }
            else
            {
                weapon.rb.velocity = weaponDir * repellingForce;
                //UI加分计算
                UIUIUI.instance.AddScore(weapon.score);
            }

        }
    }

    private void Die()
    {

    }
}
