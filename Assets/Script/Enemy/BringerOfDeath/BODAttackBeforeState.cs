using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODAttackBeforeState : EnemyState
{
    private Enemy_BringerOfDeath enemy;
    public BODAttackBeforeState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTime = Random.Range(0f, 1f);//前摇时间
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTime < 0)
        {
            stateMachine.ChangeState(enemy.attackAfterState);
        }
    }
}
