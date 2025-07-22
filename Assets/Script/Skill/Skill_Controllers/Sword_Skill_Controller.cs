using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 10f; //速度    
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cr;
    private Player player;
    private bool canRotate = true;
    private bool isReturning = false;
    [SerializeField] private float freezeTimeDuration = 0.6f; //冻结时间

    //反弹
    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed = 10f; //反弹速度
    private bool isBouncing; //是否正在反弹
    private int bounceCount; //反弹次数
    private List<Transform> enemyTargets; //敌人目标列表
    private int currentTargetIndex = 0; //当前目标索引
    [SerializeField] private float bounceRadius = 5f; //反弹半径

    //穿刺
    [Header("Pierce info")]
    private int pierceCount; //穿透次数

    //旋转
    [Header("Spin info")]
    private bool isSpinning = false; //是否正在旋转
    private bool wasStopped = false; //是否被停止
    private float maxTravelDistance = 10f; //最大移动距离
    private float spinDuration = 2f; //旋转持续时间
    private float spinTimer = 0f; //旋转计时器
    private float hitRadius = 0.5f; //命中半径
    private float hitCooldown = 0.5f; //命中冷却时间
    private float hitTimer = 0f;// 命中计时器
    private float spinMoveDistance = 1f; //移动偏移量
    private float spinMoveSpeed = 1.2f; //移动速度


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cr = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    //设置剑的初始方向、重力和玩家
    public void SetupSword(Vector2 _dir, float _gravity, Player _player , float _freezeTimeDuration)
    {
        freezeTimeDuration = _freezeTimeDuration; // 设置冻结时间
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravity;
        if (pierceCount <= 0)
            anim.SetBool("Rotation", true);

        spinMoveDistance = Mathf.Clamp(rb.velocity.x,-1,1);// 限制旋转移动距离在 -1 到 1 之间

        Invoke("DestroyMe", 7f); // 7秒后销毁剑对象
    }

    public void SetupBounce(bool _isBouncing, int _bounceCount)
    {
        isBouncing = _isBouncing;
        bounceCount = _bounceCount;
        enemyTargets = new List<Transform>();
    }

    public void SetupPierce(int _pierceCount)
    {
        pierceCount = _pierceCount;

    }
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration ,float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
        wasStopped = false;
    }

    public void ReturnSword()
    {
        //冻结剑的旋转和移动
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

        anim.SetBool("Rotation", true);
    }


    public void Update()
    {
        //使剑的朝向与速度方向一致
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        // 如果正在返回剑，处理返回逻辑
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //如果剑接近玩家，销毁剑对象
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchSword();
            }
        }
        // 如果正在反弹，处理反弹逻辑
        BounceLogic();
        // 如果正在旋转，处理旋转逻辑
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance && !wasStopped)
            {
                TriggerSpinStop();
            }
            if (wasStopped)
            {
                
                transform.position =   Vector2.MoveTowards(
                    transform.position,
                    new Vector3(spinMoveDistance + transform.position.x, transform.position.y),
                    spinMoveSpeed * Time.deltaTime); // 让剑在水平方向上移动

                spinTimer -= Time.deltaTime; // 减少旋转持续时间
                hitTimer -= Time.deltaTime; // 减少命中冷却时间
                if (spinTimer <= 0)
                {
                    isSpinning = false; // 停止旋转
                    wasStopped = false; // 重置停止状态
                    isReturning = true; // 开始返回剑
                }
                if (hitTimer <= 0)
                {
                    // 如果命中冷却时间结束，检查是否击中敌人
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, hitRadius);
                    foreach (Collider2D enemy in enemies)
                    {

                        if (enemy.TryGetComponent(out Enemy enemy1))
                        {
                            SwordSkillDamage(enemy1); // 如果碰撞物体是敌人，调用其伤害方法
                        }                     
                    }
                    hitTimer = hitCooldown; // 重置命中冷却时间
                }

            }
        }
    }

    private void TriggerSpinStop()
    {
        wasStopped = true; // 标记为已停止
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 冻结剑的旋转和移动
        spinTimer = spinDuration; // 重置旋转计时器
        hitTimer = hitCooldown; // 重置命中计时器
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            // 让剑朝向当前目标
            transform.position = Vector2.MoveTowards(
                transform.position,
                enemyTargets[currentTargetIndex].position,
                bounceSpeed * Time.deltaTime);
            // 如果剑接近当前目标，切换到下一个目标
            if (Vector2.Distance(transform.position, enemyTargets[currentTargetIndex].position) < 0.1f)
            {
                if(enemyTargets[currentTargetIndex].TryGetComponent(out Enemy enemy))
                {
                    SwordSkillDamage(enemy);
                }

                currentTargetIndex++;
                bounceCount--; ; // 减少反弹次数
                                 // 如果已经到达最后一个目标，重置索引并停止反弹
                if (currentTargetIndex >= enemyTargets.Count)
                    currentTargetIndex = 0;
                // 如果反弹次数用完，停止反弹
                if (bounceCount <= 0)
                {
                    isBouncing = false;
                    enemyTargets.Clear();
                    currentTargetIndex = 0; // 重置索引
                    isReturning = true; // 开始返回剑
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return; // 如果正在返回剑，则不处理碰撞

        if (collision.TryGetComponent(out Enemy enemy))
        {
            SwordSkillDamage(enemy);
        }


        // 如果碰撞物体是敌人，且剑可以旋转，则触发反弹逻辑
        SetupBounceTargets(collision);

        // 触发碰撞后，停止剑的旋转和移动，并将其附着到碰撞物体上
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        int SwordFacingDirection = ( transform.position.x > enemy.transform.position.x ) ? -1 : 1;
        enemy.DamageEffect(Vector2.zero, SwordFacingDirection); // 如果碰撞物体是敌人，调用其伤害方法
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration); // 冻结敌人
    }

    private void SetupBounceTargets(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            if (isBouncing && enemyTargets.Count == 0)
            {
                // 获取所有在反弹半径内的敌人
                LayerMask enemyLayerMask = LayerMask.GetMask("Enemy");
                Collider2D[] enemyList = Physics2D.OverlapCircleAll(
                    transform.position, bounceRadius, enemyLayerMask);

                foreach (Collider2D enemy in enemyList)
                {
                    enemyTargets.Add(enemy.transform);
                }
                // 也可以使用 LINQ 来简化添加逻辑
                //enemyTargets.AddRange(      // 将集合批量添加到 enemyTargets
                //    enemyList.Select(       // 对 enemyList 进行转换
                //        e => e?.transform   // 对每个元素 e 安全访问 transform
                //        )!                  // 空 forgiving 运算符（告诉编译器不为 null）
                //    );
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        // 穿刺次数大于0且碰撞物体是敌人，则减少穿刺次数
        if (pierceCount > 0 && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            pierceCount--;
            return;
        }

        canRotate = false;// 禁止剑的前进
        cr.enabled = false;// 禁用碰撞器，防止后续碰撞
        rb.isKinematic = true;// 设置刚体为运动学模式，停止物理模拟
        rb.constraints = RigidbodyConstraints2D.FreezeAll;// 冻结剑的旋转和移动

        //如果是反弹状态，且敌人目标列表不为空，则不附着到碰撞物体上
        if (isBouncing && enemyTargets.Count > 0)
            return;

        //如果是旋转状态，且剑正在旋转，则不附着到碰撞物体上
        if (isSpinning)
        {
            TriggerSpinStop();
            return;
        }

        //停止剑的旋转,将其附着到碰撞物体上
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}

