using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.attackState.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(29);
        CloseCounterAttackWindow();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                //player.Damage(Vector2.zero, enemy.facingDirection);
                enemy.stats.DoDamage(player.stats);
            }
        }
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
