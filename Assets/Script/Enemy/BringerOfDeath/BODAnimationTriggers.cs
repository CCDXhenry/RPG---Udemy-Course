using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BODAnimationTriggers : MonoBehaviour
{
    private Enemy_BringerOfDeath enemy => GetComponentInParent<Enemy_BringerOfDeath>();

    /// <summary>
    /// 动作完成通用方法
    /// </summary>
    private void FinishAnimation()
    {
        enemy.AnimationTrigger();
    }
    private void FinishAttackAfter()
    {
        enemy.attackAfterState.AnimationFinishTrigger();
    }

    private void FinishAttack()
    {
        enemy.attackState.AnimationFinishTrigger();
    }

    private void FinishTeleportBefore()
    {
        enemy.teleportBeforeState.AnimationFinishTrigger();
    }
    private void FinishTeleportAfter()
    {
        enemy.teleportAfterState.AnimationFinishTrigger();
    }

    private void FinishDead()
    {
        enemy.deadState.AnimationFinishTrigger();
        AudioManager.instance.PlayBGM(0);
        Destroy(enemy.gameObject);
    }

    private void AttackTrigger()
    {
        CloseCounterAttackWindow();
        AudioManager.instance.PlaySFX(1);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                enemy.stats.DoDamage(player.stats);
                AudioManager.instance.PlaySFX(15);

            }
        }
    }

    private void OpenCounterAttackWindow()
    {
        enemy.OpenCounterAttackWindow();
    }
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();

    
}
