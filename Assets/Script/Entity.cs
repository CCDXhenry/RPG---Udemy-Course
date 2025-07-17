using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; } // Placeholder for future FX

    public SpriteRenderer sr { get; private set; } // Placeholder for future sprite effects

    #endregion

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public Transform wallCheck;
    [SerializeField] private float wallCheckDistance = 0.2f;


    [Header("Facing info")]
    private bool facingRight = true;
    public int facingDirection { get; private set; } = 1; // 1 for right, -1 for left

    [Header("Knockback info")]
    [SerializeField] public Vector2 knockbackDirection;
    [SerializeField] public float knockbackDuration;
    public bool isKnocked;

    // Start is called before the first frame update
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        Debug.Log(gameObject.name + " was damage");
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * -facingDirection, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    //人物震动协程
    protected IEnumerator Vibrate(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            transform.position = new Vector3(transform.position.x + Random.Range(-0.05f, 0.05f), transform.position.y + Random.Range(-0.05f, 0.05f), transform.position.z);
            yield return null;
        }
    }

    public void SetVelocity(float x, float y)
    {
        if (isKnocked) return; // Prevent movement while knocked
        rb.velocity = new Vector2(x, y);
        FlipController(x);
    }

    public bool IsGroundedDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

    public bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        facingDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            Flip();
        }
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }
}
