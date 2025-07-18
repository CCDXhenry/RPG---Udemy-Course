using Assets.Script.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    [Header("Move info")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float jumpForce = 10f;
    [Header("Dash info")]
    [SerializeField] public float dashSpeed = 25f;
    [SerializeField] public float dashDuration = 0.25f;
    public float dashDir { get; private set; } = 1f;

    [Header("Attack info")]
    public int comboCounts = 0;
    [SerializeField] public Vector2[] attackMovement;

    //人物忙碌状态
    private bool _isBusy;
    public bool isBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
        }
    }
    private bool isVibrating = false; // 震动状态标志

    public float counterAttackDuration;
    public SkillManager skill { get;private set; }

    public GameObject sword { get; private set; }
    public float swordReturnImpact = 10f; // Placeholder for future sword return impact

    #region States

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }

    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }

    public PlayerAttackState attackState { get; private set; } // Placeholder for future attack state
    public PlayerCounterAttackState counterAttackState { get; private set; } // Placeholder for future counter attack state
    public PlayerAimSwordState aimSwordState { get; private set; } // Placeholder for future aim sword state
    public PlayerCatchSwordState catchSwordState { get; private set; } // Placeholder for future catch sword state
    public PlayerBlackholeState blackholeState { get; private set; } // Placeholder for future blackhole state
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        airState = new PlayerAirState(stateMachine, this, "Jump");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        wallSlideState = new PlayerWallSlideState(stateMachine, this, "WallSlide");
        attackState = new PlayerAttackState(stateMachine, this, "Attack"); // Placeholder for future attack state
        counterAttackState = new PlayerCounterAttackState(stateMachine, this, "CounterAttack"); // Placeholder for future counter attack state
        aimSwordState = new PlayerAimSwordState(stateMachine, this, "AimSword"); // Placeholder for future aim sword state
        catchSwordState = new PlayerCatchSwordState(stateMachine, this, "CatchSword"); // Placeholder for future catch sword state
        blackholeState = new PlayerBlackholeState(stateMachine, this, "Jump"); // Placeholder for future blackhole state
    }

    protected override void Start()
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
        if (Input.GetKeyDown(KeyCode.F))
        {
            SkillManager.instance.crystal.CanUseSkill(true);
        }
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchSword()
    {        
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(seconds);
        isBusy = false;

    }

    public void CheckForDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)  && !isBusy)
        {
            if(!skill.dash.CanUseSkill(true))
                return; // 如果技能不可用，直接返回
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0) // If no horizontal input, default to facing direction
            {
                dashDir = facingDirection;
            }
            stateMachine.ChangeState(dashState);
        }
    }

    //人物震动协程
    public  IEnumerator Vibrate(float duration)
    {
        if (isVibrating) yield break;
        isVibrating = true;
        stateMachine.ChangeState(idleState);
        StartCoroutine(BusyFor(duration));
        Vector3 originalPosition = transform.position; // 值类型复制，后续不再更新

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // 随机偏移X轴
            transform.position = new Vector3(
                originalPosition.x + Random.Range(-0.05f, 0.05f),
                transform.position.y,
                transform.position.z
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(originalPosition.x, transform.position.y, transform.position.z); // 精确还原
        yield return new WaitForSeconds(0.5f); // 短暂停顿
        isVibrating = false; // 重置震动状态
    }
}
