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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
