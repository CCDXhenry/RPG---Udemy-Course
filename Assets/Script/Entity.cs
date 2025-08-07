using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(CapsuleCollider2D))]


public class Entity : MonoBehaviour
{
    public string entityName;
    #region Components
    public Animator anim { get; private set; } // 动画控制器，用于处理实体的动画状态
    public Rigidbody2D rb { get; private set; } // 刚体2D组件，用于处理物理运动和碰撞
    public EntityFX fx { get; private set; } // 实体特效脚本，用于处理闪烁、受伤等视觉效果

    public SpriteRenderer sr { get; private set; } // 精灵渲染器，用于显示实体的外观

    public CharacterStats stats { get; private set; } // 人物属性脚本，包含生命值、伤害等信息
    public CapsuleCollider2D cr { get; private set; }

    #endregion
    #region Event
    public System.Action onFlipped;
    #endregion

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.2f;
    public Vector3 lastGroundCheckTransposition;

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

    public int bossStage = 0;//boss阶段

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
        stats = GetComponent<CharacterStats>();
        cr = GetComponentInChildren<CapsuleCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public virtual void DamageEffect(Vector2 _knockbackVector, int facingDirection)
    {
        if (_knockbackVector == Vector2.zero)
            _knockbackVector = knockbackVector;

        //Debug.Log(gameObject.name + " was damage " + _knockbackVector);
        fx.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockback(_knockbackVector, facingDirection));
    }

    protected virtual IEnumerator HitKnockback(Vector2 _knockbackDirection, int facingDirection)
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

    public virtual bool IsGroundedDetected()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }


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
        onFlipped?.Invoke();// 如果有订阅者，调用它们

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
    public void SetDefaultFacingDirection()
    {
        facingRight = !facingRight;
        facingDirection *= -1;
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

    public virtual void Die()
    {

    }
}
