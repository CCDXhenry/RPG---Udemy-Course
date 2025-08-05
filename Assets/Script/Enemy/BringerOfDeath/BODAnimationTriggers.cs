using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BODAnimationTriggers : MonoBehaviour
{
    private Enemy_BringerOfDeath enemy => GetComponentInParent<Enemy_BringerOfDeath>();

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

    private void AttackTrigger()
    {
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
}
