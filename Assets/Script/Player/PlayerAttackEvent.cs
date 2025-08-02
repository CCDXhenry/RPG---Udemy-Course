using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEvent : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AttackEnd()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                //enemy.DamageEffect(Vector2.zero, player.facingDirection);
                player.stats.DoDamage(enemy.stats, 0);
                Inventory.instance.GetEqiupmentEffects(EquipmentType.Weapon)?.ExecuteItemEffect(enemy.transform);
            }
                
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
