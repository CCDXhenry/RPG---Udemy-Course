using System.Collections;
using UnityEngine;


public class BODGroundState : EnemyState
{

    protected Enemy_BringerOfDeath enemy;
    private Transform playerTrans;
    private float minDistance = 3f; // Minimum distance to consider the player close enough to attack
    protected float distanceToPlayer;
    protected float distanceToPlayerX;
    
    public BODGroundState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        playerTrans = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.isBattle)
        {
            //获取与玩家的距离
            distanceToPlayer = Vector2.Distance(playerTrans.position, enemy.transform.position);
            distanceToPlayerX = playerTrans.position.x - enemy.transform.position.x;

            //如果距离大于触发范围的一半，就切换到teleportState瞬移状态
            if (distanceToPlayer > enemy.battleRangeTrigger.boxCr.size.x * 0.5f)
            {
                enemy.teleportEnum = BODTeleportEnum.attack;
                stateMachine.ChangeState(enemy.teleportBeforeState);
                Debug.Log("距离大于触发范围的一半，切换到teleportState瞬移状态");
                return;
            }

            //判断玩家是否在敌人的攻击范围内以及攻击冷却是否转好，切换到attackState攻击状态
            if (distanceToPlayer <= Mathf.Abs(enemy.attackDistance) && (Time.time - enemy.lastTimeAttack) > enemy.attackCooldown)
            {
                stateMachine.ChangeState(enemy.attackState);
                Debug.Log("玩家在敌人的攻击范围内以及攻击冷却转好，切换到attackState攻击状态");
                return;
            }

            //判断是否触碰到地面或者墙壁,切换到teleportState瞬移状态
            if (!enemy.IsGroundedDetected() || enemy.IsWallDetected())
            {
                stateMachine.ChangeState(enemy.teleportBeforeState);
                Debug.Log("判断是否触碰到地面或者墙壁,切换到teleportState瞬移状态");
                return;
            }

            //判断enemy的面朝方向
            if (Mathf.Abs(distanceToPlayerX) > minDistance)
            {
                if (distanceToPlayerX > 0 && enemy.facingDirection < 0)
                {
                    enemy.Flip();
                }
                else if (distanceToPlayerX < 0 && enemy.facingDirection > 0)
                {
                    enemy.Flip();
                }
                if(animBoolName != "Move")
                    enemy.stateMachine.ChangeState(enemy.moveState);
            }
            else
            {
                if(animBoolName != "Idle")
                    enemy.stateMachine.ChangeState(enemy.idleState);
            }
        }
    }
}
