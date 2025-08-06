using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private CrystalTags crystalTag;// 水晶技能标签，用于区分不同的水晶技能类型
    private float crystalTimer;
    private float crystalDuration; // Duration for which the crystal remains active
    private float maxDistance; // Maximum distance the crystal can be placed from the player
    private bool canGrow;
    private float growSpeed;
    private float maxSize;


    private bool isExploded = false;
    private bool _isExploding = false;
    public bool isExploding
    {
        get => _isExploding;
        set => _isExploding = value;
    }

    private GameObject currentCrystal; // Prefab for the crystal
    private Animator anim;
    private CircleCollider2D circleCollider2D;
    private Vector3 InitialCrystalTransform;

    [Header("Move Skill Info")]
    private float moveSpeed; // 移动水晶技能速度
    private float moveDistance; // 移动水晶技能距离
    private Transform closestEnemy;// 最近的敌人
    private Vector2 moveDirection; // 移动水晶技能方向
    private Vector2 moveTargetPosition; // 目标位置
    private void Awake()
    {
        anim = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {

    }

    public void SetupCrystal(CrystalTags _crystalTag, GameObject _currentCrystal, float _crystalDuration, float _maxDistance, bool _canGrow, float _growSpeed, float _maxSize)
    {
        crystalTag = _crystalTag;
        currentCrystal = _currentCrystal;
        crystalDuration = _crystalDuration;
        maxDistance = _maxDistance;
        canGrow = _canGrow;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        InitialCrystalTransform = currentCrystal.transform.position;
        crystalTimer = crystalDuration;
    }
    public void SetupSwitchoverCrystal()
    {

    }

    //设置移动水晶
    public void SetupMoveCrystal(float _moveSpeed, float _moveDistance, Vector2 _moveDirection)
    {
        moveSpeed = _moveSpeed;
        moveDistance = _moveDistance;
        moveDirection = _moveDirection;
        moveTargetPosition = new(InitialCrystalTransform.x + (moveDirection.x * maxDistance), InitialCrystalTransform.y + (moveDirection.y * maxDistance));
        //找到最近的敌人
        FindClosestEnemy();
    }

    private void FindClosestEnemy()
    {
        LayerMask enemyLayerMask = LayerMask.GetMask("Enemy");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(InitialCrystalTransform, maxDistance, enemyLayerMask);
        if (colliders.Length > 0)
        {
            closestEnemy = colliders[0].transform;
            float closestDistance = Vector2.Distance(InitialCrystalTransform, closestEnemy.position);
            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(InitialCrystalTransform, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.transform;
                }
            }
        }
    }

    private void Update()
    {
        crystalTimer -= Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(InitialCrystalTransform, currentCrystal.transform.position);
        if (crystalTimer <= 0f || maxDistance - 1f <= distanceToPlayer)
        {
            isExploding = true;
        }
        if (crystalTag == CrystalTags.Move)
        {
            if (closestEnemy == null)
            {
                transform.position = Vector2.MoveTowards(transform.position, moveTargetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, closestEnemy.position) < 1f)
                {
                    isExploding = true; // 如果水晶到达目标位置，则触发爆炸
                }
            }

        }
        if (isExploding)
            ProcessCrystalExplosion();

    }

    private void ProcessCrystalExplosion()
    {
        if (!isExploded)
        {//确保爆炸动画只触发一次
            anim.SetTrigger("Explode");
            isExploded = true;
            AudioManager.instance.StopSFX(25);
            AudioManager.instance.PlaySFX(24);
            
        }

        if (canGrow)
        {
            currentCrystal.transform.localScale = Vector2.Lerp(currentCrystal.transform.localScale, Vector2.one * maxSize, growSpeed * Time.deltaTime);
        }
    }

    public void AnimationAttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(currentCrystal.transform.position, circleCollider2D.radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                Vector2 knockbackVector = new Vector2(5, 0);
                int damageFacingDirection = (currentCrystal.transform.position.x > enemy.transform.position.x) ? -1 : 1;
                //enemy.DamageEffect(knockbackVector, damageFacingDirection);
                var playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
                int totalDamage = (playerStats.damage.GetValue() + playerStats.strength.GetValue()) * Random.Range(5, 8);
                playerStats.DoDamage(enemy.GetComponent<EnemyStats>(), totalDamage, damageFacingDirection);
            }
        }
    }

    public void AnimationFinishTrigger()
    {
        DestroyCrystal();
    }

    private void DestroyCrystal()
    {
        Destroy(gameObject);
        Destroy(currentCrystal);
    }
}
