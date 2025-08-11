using Assets.Script.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Entity
{
    public VirtualJoystick joystick;
    [SerializeField] private GameObject hitFXPrefab;
    public Tilemap groundTilemap; // 绑定地面的Tilemap
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


    public event System.Action ResetMultistageJumpCounter;//重置连跳计数

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
    public SkillManager skill { get; private set; }

    public GameObject sword { get; private set; }
    public float swordReturnImpact = 10f; // Placeholder for future sword return impact

    //人物属性
    public PlayerStats playerStats { get; private set; }

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
    public PlayerDeadState deadState { get; private set; } // Placeholder for future dead state
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
        deadState = new PlayerDeadState(stateMachine, this, "Die"); // Placeholder for future dead state
    }

    protected override void Start()
    {
        base.Start();
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
        playerStats = GetComponent<PlayerStats>();
    }

    protected override void Update()
    {
        base.Update();
        if (Time.timeScale == 0 || playerStats.isDead)
        {
            return;
        }

        stateMachine.currentState.Update();
        if (isBusy)
        {
            return;
        }
        //冲刺技能
        CheckForDashInput();

        //水晶技能
        if (Input.GetKeyDown(KeyCode.F))
        {
            SkillManager.instance.crystal.CanUseSkill(true);
        }

        // 连跳技能
        CheckForMultistageJumpInput();

        // 黑洞技能
        if (Input.GetKeyDown(KeyCode.R) && SkillManager.instance.blackhole.CanUseSkill(false))
        {
            stateMachine.ChangeState(blackholeState);
        }
    }

    public override void CreateHitFX(Transform _target)
    {
        float zRotation = Random.Range(-60, 60);
        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);

        var hitPosition = new Vector3(_target.position.x + xPosition, transform.position.y + yPosition);
        GameObject newHitFX = Instantiate(hitFXPrefab, hitPosition, Quaternion.identity);
        if (facingDirection < 0)
        {
            newHitFX.transform.Rotate(0, 180, zRotation);
        }
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchSword()
    {
        //防止打断黑洞技能
        if (stateMachine.currentState.animBoolName != "Jump" && !isBusy)
        {
            stateMachine.ChangeState(catchSwordState);
        }
        AudioManager.instance.StopSFX(19);
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBusy)
        {
            if (!skill.dash.CanUseSkill(true))
                return; // 如果技能不可用，直接返回
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0) // If no horizontal input, default to facing direction
            {
                dashDir = facingDirection;
            }
            stateMachine.ChangeState(dashState);
        }
    }
    public void CheckForMultistageJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isBusy && !IsGroundedDetected())
        {
            skill.multistageJump.CanUseSkill(true);
        }
    }

    //人物震动协程
    public IEnumerator Vibrate(float duration)
    {
        if (isVibrating || !IsGroundedDetected()) yield break;
        isVibrating = true;
        stateMachine.ChangeState(idleState);
        StartCoroutine(BusyFor(duration));
        float originalPositionX = rb.position.x;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // 随机偏移X轴
            rb.MovePosition(new Vector3(
                originalPositionX + Random.Range(-0.05f, 0.05f),
                rb.position.y
            ));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(new Vector3(originalPositionX, rb.position.y)); // 精确还原
        yield return new WaitForSeconds(0.5f);
        isVibrating = false; // 重置震动状态
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Vector3 tileCenter = GetTileCenterTrans();
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(tileCenter, 0.1f);
    }
    public override bool IsGroundedDetected()
    {
        bool isGrounded = base.IsGroundedDetected();
        if (isGrounded)
        {
            var tileCenter = GetTileCenterTrans();
            float yOffset = -0.17f;//人物模型与死亡模型的y轴偏移量
            lastGroundCheckTransposition = new Vector3(tileCenter.x, transform.position.y + yOffset, transform.position.z);//保存上一次检测到的地面位置
            ResetMultistageJumpCounter?.Invoke();//重置连跳计数
        }
        return isGrounded;

    }

    /// <summary>
    /// 获取人物所在格子中心点,用于死区掉魂坐标判断(实测下来,好像没啥用)
    /// </summary>
    /// <returns></returns>
    private Vector3 GetTileCenterTrans()
    {
        Vector3Int cellPos = groundTilemap.WorldToCell(transform.position);
        Vector3 tileCenter = groundTilemap.GetCellCenterWorld(cellPos);
        return tileCenter;
    }

}
