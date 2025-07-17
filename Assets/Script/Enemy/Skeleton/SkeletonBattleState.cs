using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private Transform player;
    private int moveDir = 1; // 1 for right, -1 for left
    public SkeletonBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    // Update is called once per frame
    public override void Enter()
    {
        base.Enter();
        player =  PlayerManager.instance.player.transform;
        stateTime = enemy.battleTime;
        enemy.lastTimeAttack = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(player.position.x > enemy.transform.position.x)
            moveDir = 1; // Move right
        else
            moveDir = -1; // Move left
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        //Debug.Log(CanAttack());
        if (enemy.IsPlayerDetected())
        {
            stateTime = enemy.battleTime;
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
               if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else if(stateTime < 0 || Vector2.Distance(player.transform.position,enemy.transform.position) >10 )
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
    public virtual bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttack + enemy.attackCooldown)
        {
            return true;
        }
        return false;
    }


}
