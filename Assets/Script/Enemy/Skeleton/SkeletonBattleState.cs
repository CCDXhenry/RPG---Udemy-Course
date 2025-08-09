using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private Transform player;
    private int moveDir = 1; // 1 for right, -1 for left
    private float minDistance = 0.05f; // Minimum distance to consider the player close enough to attack
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
        enemy.lastTimeAttack = Time.time - enemy.attackCooldown;

        if (PlayerManager.instance.player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        float distanceToPlayer = Mathf.Abs(player.position.x - enemy.transform.position.x);
        if (distanceToPlayer > minDistance)
        {
            moveDir = player.position.x > enemy.transform.position.x ? 1 : -1;
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        }
        
        if (enemy.isBattle)
        {
            
            if (enemy.CanAttackToPlayer())
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
                    
            }
        }
        else
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        if (enemy.IsWallDetected() || !enemy.IsGroundedDetected() || !enemy.IsSameGround())
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
