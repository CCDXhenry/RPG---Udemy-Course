using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTime = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0,0);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                //判断是否面向玩家
                bool facingOpposite = enemy.facingDirection != player.facingDirection;

                //判断是否在攻击范围内
                float dist = Vector2.Distance(player.attackCheck.position, enemy.attackCheck.position);
                float combinedRange = player.attackCheckRadius + enemy.attackCheckRadius;
                bool attackRangeOverlap = dist <= combinedRange;

                if (enemy.CanBeStunned() && facingOpposite && attackRangeOverlap)
                {
                    stateTime = 10f;//弹反动作持续时间,防止招架动作结束改变状态
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                    AudioManager.instance.PlaySFX(0);
                }
            }
        }
        if (stateTime < 0 || triggerCalled)//弹反动作结束
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
