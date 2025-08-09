using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(ItemDrop))]
[RequireComponent(typeof(EnemyStats))]
public class Enemy : Entity
{

    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;


    [Header("Move Info")]
    public float moveSpeed;
    private float originalMoveSpeed;
    public float idleTime;
    public float bufferPeriod;// 缓冲时间

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttack;
    public float battleTime;

    [Header("Battle Range Info")]
    [SerializeField] private BoxCollider2D battleRange;//触发竞技范围
    public bool isBattle;//是否为竞技状态
    public BattleRangeTrigger battleRangeTrigger;//活动场地范围

    public LayerMask PlayerMask;
    private Player player;

    #region States

    public EnemyStateMachine stateMachine { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        originalMoveSpeed = moveSpeed;

        battleRangeTrigger.battleRangeonTriggerEnter += EnterBattle;
        battleRangeTrigger.battleRangeonTriggerExit += ExitBattle;
    }

    protected override void Start()
    {
        base.Start();
        player = PlayerManager.instance.player;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void FreezeTimer(bool _timeFroze)
    {
        if (_timeFroze)
        {
            moveSpeed = 0f;
            anim.speed = 0f;
        }
        else
        {
            moveSpeed = originalMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual IEnumerator FreezeTimerFor(float _second)
    {
        FreezeTimer(true);
        yield return new WaitForSeconds(_second);
        FreezeTimer(false);
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected() =>
        Physics2D.Raycast(transform.position, Vector2.right * facingDirection, 50, PlayerMask);

    public virtual bool CanAttackToPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                return true;
            }
        }
        return false;
    }
    public virtual bool IsSameGround()
    {   //检测是否在同一高度上
        float yOffset = Mathf.Abs(transform.position.y - player.transform.position.y);
        if (yOffset > 1.0f && player.leaveGroundTime == 0)
        {
            return false;
        }
        return true;
    }
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();


    protected virtual void EnterBattle()
    {

    }

    protected virtual void ExitBattle()
    {

    }
    public override void Die()
    {
        base.Die();
        counterImage.SetActive(false);
    }

    private void OnDestroy()
    {
        battleRangeTrigger.battleRangeonTriggerEnter -= EnterBattle;
        battleRangeTrigger.battleRangeonTriggerExit -= ExitBattle;
    }
}
