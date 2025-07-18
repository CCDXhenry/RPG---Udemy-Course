using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 10f; //�ٶ�    
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cr;
    private Player player;
    private bool canRotate = true;
    private bool isReturning = false;
    [SerializeField] private float freezeTimeDuration = 0.6f; //����ʱ��

    //����
    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed = 10f; //�����ٶ�
    private bool isBouncing; //�Ƿ����ڷ���
    private int bounceCount; //��������
    private List<Transform> enemyTargets; //����Ŀ���б�
    private int currentTargetIndex = 0; //��ǰĿ������
    [SerializeField] private float bounceRadius = 5f; //�����뾶

    //����
    [Header("Pierce info")]
    private int pierceCount; //��͸����

    //��ת
    [Header("Spin info")]
    private bool isSpinning = false; //�Ƿ�������ת
    private bool wasStopped = false; //�Ƿ�ֹͣ
    private float maxTravelDistance = 10f; //����ƶ�����
    private float spinDuration = 2f; //��ת����ʱ��
    private float spinTimer = 0f; //��ת��ʱ��
    private float hitRadius = 0.5f; //���а뾶
    private float hitCooldown = 0.5f; //������ȴʱ��
    private float hitTimer = 0f;// ���м�ʱ��
    private float spinMoveDistance = 1f; //�ƶ�ƫ����
    private float spinMoveSpeed = 1.2f; //�ƶ��ٶ�


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

    //���ý��ĳ�ʼ�������������
    public void SetupSword(Vector2 _dir, float _gravity, Player _player , float _freezeTimeDuration)
    {
        freezeTimeDuration = _freezeTimeDuration; // ���ö���ʱ��
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravity;
        if (pierceCount <= 0)
            anim.SetBool("Rotation", true);

        spinMoveDistance = Mathf.Clamp(rb.velocity.x,-1,1);// ������ת�ƶ������� -1 �� 1 ֮��

        Invoke("DestroyMe", 7f); // 7������ٽ�����
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
        //���ὣ����ת���ƶ�
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

        anim.SetBool("Rotation", true);
    }


    public void Update()
    {
        //ʹ���ĳ������ٶȷ���һ��
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        // ������ڷ��ؽ����������߼�
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            //������ӽ���ң����ٽ�����
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchSword();
            }
        }
        // ������ڷ������������߼�
        BounceLogic();
        // ���������ת��������ת�߼�
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
                    spinMoveSpeed * Time.deltaTime); // �ý���ˮƽ�������ƶ�

                spinTimer -= Time.deltaTime; // ������ת����ʱ��
                hitTimer -= Time.deltaTime; // ����������ȴʱ��
                if (spinTimer <= 0)
                {
                    isSpinning = false; // ֹͣ��ת
                    wasStopped = false; // ����ֹͣ״̬
                    isReturning = true; // ��ʼ���ؽ�
                }
                if (hitTimer <= 0)
                {
                    // ���������ȴʱ�����������Ƿ���е���
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, hitRadius);
                    foreach (Collider2D enemy in enemies)
                    {

                        if (enemy.TryGetComponent(out Enemy enemy1))
                        {
                            SwordSkillDamage(enemy1); // �����ײ�����ǵ��ˣ��������˺�����
                        }                     
                    }
                    hitTimer = hitCooldown; // ����������ȴʱ��
                }

            }
        }
    }

    private void TriggerSpinStop()
    {
        wasStopped = true; // ���Ϊ��ֹͣ
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // ���ὣ����ת���ƶ�
        spinTimer = spinDuration; // ������ת��ʱ��
        hitTimer = hitCooldown; // �������м�ʱ��
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            // �ý�����ǰĿ��
            transform.position = Vector2.MoveTowards(
                transform.position,
                enemyTargets[currentTargetIndex].position,
                bounceSpeed * Time.deltaTime);
            // ������ӽ���ǰĿ�꣬�л�����һ��Ŀ��
            if (Vector2.Distance(transform.position, enemyTargets[currentTargetIndex].position) < 0.1f)
            {
                if(enemyTargets[currentTargetIndex].TryGetComponent(out Enemy enemy))
                {
                    SwordSkillDamage(enemy);
                }

                currentTargetIndex++;
                bounceCount--; ; // ���ٷ�������
                                 // ����Ѿ��������һ��Ŀ�꣬����������ֹͣ����
                if (currentTargetIndex >= enemyTargets.Count)
                    currentTargetIndex = 0;
                // ��������������ֹ꣬ͣ����
                if (bounceCount <= 0)
                {
                    isBouncing = false;
                    enemyTargets.Clear();
                    currentTargetIndex = 0; // ��������
                    isReturning = true; // ��ʼ���ؽ�
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return; // ������ڷ��ؽ����򲻴�����ײ

        if (collision.TryGetComponent(out Enemy enemy))
        {
            SwordSkillDamage(enemy);
        }


        // �����ײ�����ǵ��ˣ��ҽ�������ת���򴥷������߼�
        SetupBounceTargets(collision);

        // ������ײ��ֹͣ������ת���ƶ��������丽�ŵ���ײ������
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        int SwordFacingDirection = ( transform.position.x > enemy.transform.position.x ) ? -1 : 1;
        enemy.Damage(Vector2.zero, SwordFacingDirection); // �����ײ�����ǵ��ˣ��������˺�����
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration); // �������
    }

    private void SetupBounceTargets(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            if (isBouncing && enemyTargets.Count == 0)
            {
                // ��ȡ�����ڷ����뾶�ڵĵ���
                LayerMask enemyLayerMask = LayerMask.GetMask("Enemy");
                Collider2D[] enemyList = Physics2D.OverlapCircleAll(
                    transform.position, bounceRadius, enemyLayerMask);

                foreach (Collider2D enemy in enemyList)
                {
                    enemyTargets.Add(enemy.transform);
                }
                // Ҳ����ʹ�� LINQ ��������߼�
                //enemyTargets.AddRange(      // ������������ӵ� enemyTargets
                //    enemyList.Select(       // �� enemyList ����ת��
                //        e => e?.transform   // ��ÿ��Ԫ�� e ��ȫ���� transform
                //        )!                  // �� forgiving ����������߱�������Ϊ null��
                //    );
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        // ���̴�������0����ײ�����ǵ��ˣ�����ٴ��̴���
        if (pierceCount > 0 && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            pierceCount--;
            return;
        }

        canRotate = false;// ��ֹ����ǰ��
        cr.enabled = false;// ������ײ������ֹ������ײ
        rb.isKinematic = true;// ���ø���Ϊ�˶�ѧģʽ��ֹͣ����ģ��
        rb.constraints = RigidbodyConstraints2D.FreezeAll;// ���ὣ����ת���ƶ�

        //����Ƿ���״̬���ҵ���Ŀ���б�Ϊ�գ��򲻸��ŵ���ײ������
        if (isBouncing && enemyTargets.Count > 0)
            return;

        //�������ת״̬���ҽ�������ת���򲻸��ŵ���ײ������
        if (isSpinning)
        {
            TriggerSpinStop();
            return;
        }

        //ֹͣ������ת,���丽�ŵ���ײ������
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}

