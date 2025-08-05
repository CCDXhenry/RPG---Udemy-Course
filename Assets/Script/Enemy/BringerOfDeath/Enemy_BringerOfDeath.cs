using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum BODTeleportEnum
{
    attack,
    blackHand,
    dodge
}
public class Enemy_BringerOfDeath : Enemy
{
    [SerializeField] private BoxCollider2D battleRange;//触发竞技范围
    public bool isBattle;//是否为竞技状态
    public BattleRangeTrigger battleRangeTrigger;//活动场地范围

    //获取boss血条UI
    [SerializeField] private UI_Controller ui;
    public string bossName;
    //获取活动范围
    [SerializeField] private PolygonCollider2D arenaCollider;

    //瞬移偏移量
    [SerializeField] private float offsetDistanceX;
    [SerializeField] private float offsetDistanceY;
    //人物坐标距离地面高度
    [SerializeField] private float distanceGrounded = 2.5f;

    //瞬移安全坐标
    [Header("Teleport Info")]
    [SerializeField] private Vector2 saveTransPosition;

    [Range(0,1)]
    public float teleportProbability = 0.5f;//瞬移技能触发概率
    private Vector2 lastCheckPos;         // 记录上一次检测的位置
    private Vector2 lastCheckSize;       // 记录上一次检测的尺寸
    private CapsuleDirection2D lastDir;   // 记录上一次检测的方向
    private Collider2D lastHit;           // 记录上一次检测到的碰撞体

    public BODTeleportEnum teleportEnum;
    #region States
    public BODIdleState idleState { get; private set; }
    public BODMoveState moveState { get; private set; }
    public BODAttackState attackState { get; private set; }

    public BODTeleportBeforeState teleportBeforeState { get; private set; }
    public BODTeleportAfterState teleportAfterState { get; private set; }
    public BODBlackHandState blackHandState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new BODIdleState(stateMachine, this, "Idle", this);
        moveState = new BODMoveState(stateMachine, this, "Move", this);
        attackState = new BODAttackState(stateMachine, this, "Attack", this);
        teleportBeforeState = new BODTeleportBeforeState(stateMachine, this, "TeleportBefore", this);
        teleportAfterState = new BODTeleportAfterState(stateMachine, this, "TeleportAfter", this);
        blackHandState = new BODBlackHandState(stateMachine, this, "BlackHand", this);

        battleRangeTrigger.battleRangeonTriggerEnter += SetBattle;
        battleRangeTrigger.battleRangeonTriggerExit += SetBattle;
        SetDefaultFacingDirection();
    }

    private void Set()
    {
        throw new System.NotImplementedException();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SetBattle()
    {
        if (isBattle)
        {
            isBattle = false;
        }
        else
        {
            isBattle = true;
            ui.inGameUI.GetComponent<UI_InGame>().enemyStats = GetComponent<EnemyStats>();
            ui.inGameUI.GetComponent<UI_InGame>().bossName.text = bossName;
        }
    }


    private void OnDestroy()
    {
        battleRangeTrigger.battleRangeonTriggerEnter -= SetBattle;
        battleRangeTrigger.battleRangeonTriggerExit -= SetBattle;
    }

    /// <summary>
    /// 检查周围是否有物体,防止瞬移卡在墙中
    /// </summary>
    /// <returns></returns>
    public bool SomethingIsAround(Vector2 transPosion)
    {
        lastCheckPos = transPosion;
        lastCheckSize = cr.size;
        lastDir = cr.direction;

        Vector2 size = cr.size;
        CapsuleDirection2D direction = cr.direction;
        lastHit = Physics2D.OverlapCapsule(
            transPosion,
            size,
            direction,
            0,
            groundLayer
            );
        return lastHit != null;
    }

    /// <summary>
    /// 安全瞬移（自动避开墙体）
    /// </summary>
    public Vector2 SafeTeleport(Vector2 targetPosition)
    {
        // 处理瞬移坐标高度差
        while (IsGroundContact(targetPosition))
        {
            targetPosition.y += offsetDistanceY;
        }

        //// 获取活动区域中心点
        //Vector2 arenaCenter = arenaCollider.bounds.center;
        ////判断目标位置在左侧还是右侧
        //bool isLeftSide = targetPosition.x < arenaCenter.x;

        //随机偏移方向
        bool isLeftSide = Random.Range(0, 2) == 0;
        // 偏移次数
        float offsetAttempts = 0;

        Vector2 adjustedPosition = targetPosition;

        // 判断瞬移的位置周围是否有物体并且在活动范围内,尝试往isLeftSide方向进行位置偏移
        while ((SomethingIsAround(adjustedPosition) || !IsPositionValid(adjustedPosition)) && offsetAttempts < 5)
        {
            Debug.LogWarning("瞬移位置卡墙，正在调整...");
            adjustedPosition.x += isLeftSide ? -offsetDistanceX : offsetDistanceX;
            offsetAttempts++;
        }
        offsetAttempts = 0;
        //尝试往isLeftSide反方向偏移
        while ((SomethingIsAround(adjustedPosition) || !IsPositionValid(adjustedPosition)) && offsetAttempts < 5)
        {
            if (offsetAttempts == 0)
            {
                adjustedPosition = targetPosition;// 恢复初始坐标
            }
            Debug.LogWarning("瞬移位置卡墙，正在调整...");
            adjustedPosition.x += !isLeftSide ? -offsetDistanceX : offsetDistanceX;
            offsetAttempts++;
        }
        //如果偏移失败,返回安全坐标
        if ((SomethingIsAround(adjustedPosition) || !IsPositionValid(adjustedPosition)))
        {
            adjustedPosition = saveTransPosition;
        }

        return adjustedPosition;
    }

    /// <summary>
    /// 判断坐标是否接触地面
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private bool IsGroundContact(Vector2 targetPosition)
    {
        RaycastHit2D groundHit = Physics2D.Raycast(
           targetPosition,
           Vector2.down,
           distanceGrounded,
           groundLayer
       );
        return groundHit;
    }

    /// <summary>
    /// 检查位置是否在活动范围内
    /// </summary>
    private bool IsPositionValid(Vector2 position)
    {
        return arenaCollider.OverlapPoint(position);
    }

    /// <summary>
    /// 在Scene视图中绘制检测用的胶囊体
    /// </summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (cr == null) return;

        // 设置颜色：检测到碰撞体为红色，否则为绿色
        Gizmos.color = lastHit != null ? Color.red : Color.green;

        // 绘制胶囊体
        DrawCapsule(lastCheckPos, lastCheckSize, lastDir);
    }

    /// <summary>
    /// 绘制胶囊体形状
    /// </summary>
    private void DrawCapsule(Vector2 center, Vector2 size, CapsuleDirection2D direction)
    {
        if (size == Vector2.zero) return; // 避免初始帧绘制

        float radius, height;
        Vector2 top, bottom;

        if (direction == CapsuleDirection2D.Vertical)
        {
            // 垂直胶囊体 = 上下两个半圆 + 中间矩形
            radius = size.x * 0.5f;
            height = size.y - size.x;
            top = center + Vector2.up * (height * 0.5f);
            bottom = center - Vector2.up * (height * 0.5f);

            // 绘制半圆
            DrawWireArc(top, radius, 180, 0);
            DrawWireArc(bottom, radius, 0, 180);

            // 绘制矩形边
            Gizmos.DrawLine(
                top + Vector2.right * radius,
                bottom + Vector2.right * radius
            );
            Gizmos.DrawLine(
                top - Vector2.right * radius,
                bottom - Vector2.right * radius
            );
        }
        else
        {
            // 水平胶囊体 = 左右两个半圆 + 中间矩形
            radius = size.y * 0.5f;
            height = size.x - size.y;
            top = center + Vector2.right * (height * 0.5f);
            bottom = center - Vector2.right * (height * 0.5f);

            // 绘制半圆
            DrawWireArc(top, radius, 90, -90);
            DrawWireArc(bottom, radius, -90, 90);

            // 绘制矩形边
            Gizmos.DrawLine(
                top + Vector2.up * radius,
                bottom + Vector2.up * radius
            );
            Gizmos.DrawLine(
                top - Vector2.up * radius,
                bottom - Vector2.up * radius
            );
        }
    }
    /// <summary>
    /// 绘制圆弧（辅助方法）
    /// </summary>
    private void DrawWireArc(Vector2 center, float radius, float startAngle, float endAngle)
    {
        int segments = 10;
        float angle = startAngle;
        float arcLength = endAngle - startAngle;
        Vector2 prevPoint = center + new Vector2(
            Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            Mathf.Sin(Mathf.Deg2Rad * angle) * radius
        );

        for (int i = 1; i <= segments; i++)
        {
            angle = startAngle + arcLength * i / segments;
            Vector2 nextPoint = center + new Vector2(
                Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
                Mathf.Sin(Mathf.Deg2Rad * angle) * radius
            );
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
