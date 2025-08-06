using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODStunnedState : EnemyState
{
    protected Enemy_BringerOfDeath enemy;
    public BODStunnedState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);
        stateTime = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDirection * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelRedBlink", 0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTime < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
