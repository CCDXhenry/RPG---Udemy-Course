using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODBlackHandState : EnemyState
{
    private Enemy_BringerOfDeath enemy;
    public BODBlackHandState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.transform.position.x > PlayerManager.instance.player.transform.position.x && enemy.facingDirection > 0)
        {
            enemy.Flip();
        }
        else if(enemy.transform.position.x < PlayerManager.instance.player.transform.position.x && enemy.facingDirection < 0)
        {
            enemy.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
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
