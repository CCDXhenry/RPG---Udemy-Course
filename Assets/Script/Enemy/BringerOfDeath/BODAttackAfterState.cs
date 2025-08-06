using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODAttackAfterState : EnemyState
{
    private Enemy_BringerOfDeath enemy;
    public BODAttackAfterState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        var playerTrans = PlayerManager.instance.player.transform;
        float distanceToPlayerX = playerTrans.position.x - enemy.transform.position.x;
        if (distanceToPlayerX > 0 && enemy.facingDirection < 0)
        {
            enemy.Flip();
        }
        else if (distanceToPlayerX < 0 && enemy.facingDirection > 0)
        {
            enemy.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
