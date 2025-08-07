using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    private PlayerStats playerStats;
    public CapsuleCollider2D cr;
    [SerializeField] private int basicDamage; // 基础伤害
    [SerializeField] private int maxDamage; // 最大伤害
    [Range(0,1)]
    [SerializeField] private float minDamagePercent; // 最小伤害百分比
   //[SerializeField] private float stunDuration = 1f; // 闪电打击的眩晕持续时间
    private void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        cr = GetComponent<CapsuleCollider2D>();
        cr.enabled = false; // 初始时禁用碰撞器
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            // 计算最大伤害
            maxDamage = playerStats.damage.GetValue() + playerStats.strength.GetValue() + basicDamage;

            // 获取雷电碰撞器的中心点（世界坐标）
            Vector2 center = cr.bounds.center;

            // 计算敌人碰撞体上离雷电中心最近的点
            Vector2 closestPoint = collision.ClosestPoint(center);

            // 计算敌人受伤击退方向
            int facingDirection = (int)Mathf.Sign(closestPoint.x - center.x);

            // 计算X轴距离（绝对值）
            float distanceX = Mathf.Abs(closestPoint.x - center.x);

            // 计算伤害衰减比例（0~1）
            float maxDistance = cr.bounds.extents.x; // 碰撞器半宽度
            float damagePercent = Mathf.Clamp01(1 - distanceX / maxDistance);

            // 应用最低伤害保底
            damagePercent = Mathf.Lerp(minDamagePercent, 1f, damagePercent);

            // 最终伤害
            float finalDamage = maxDamage * damagePercent;

            // 对敌人造成伤害
            playerStats.DoDamage(enemy.GetComponent<CharacterStats>(), (int)finalDamage, facingDirection);

            Debug.Log($"击中敌人: {collision.name}, 伤害: {finalDamage} (百分比: {damagePercent * 100}%)");

        }
    }
    private void OnDrawGizmosSelected()
    {
        if (cr == null) return;

        // 绘制雷电碰撞器范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(cr.bounds.center, cr.bounds.size);

        // 标记中心点（最高伤害）
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cr.bounds.center, 0.1f);
    }


    //开启碰撞器
    public void EnableCollider()
    {
        cr.enabled = true;
    }

    //关闭碰撞器
    public void DisableCollider()
    {
        cr.enabled = false;
    }

    //删除自身
    public void DestroySelf()
    {
        var parentObject = transform.parent.gameObject;
        Destroy(gameObject);
        if (parentObject != null)
        {
            Destroy(parentObject);
        }
    }
}
