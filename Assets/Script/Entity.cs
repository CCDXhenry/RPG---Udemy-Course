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
    public Vector2 knockbackVector;
    public float knockbackDuration;
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

    public virtual void Damage(Vector2 _knockbackVector,int facingDirection)
    {
        if(_knockbackVector == Vector2.zero)
            _knockbackVector = knockbackVector;

        Debug.Log(gameObject.name + " was damage " + _knockbackVector);
        fx.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockback(_knockbackVector, facingDirection));
    }

    protected virtual IEnumerator HitKnockback(Vector2 _knockbackDirection,int facingDirection)
    {
        isKnocked = true;
        rb.velocity = new Vector2(_knockbackDirection.x * facingDirection, _knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
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
