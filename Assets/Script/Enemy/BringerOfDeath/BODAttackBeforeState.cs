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
        stateTime = Random.Range(0f, 0.5f);//前摇时间
        enemy.AttackAfterCounts();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!triggerCalled)
        {
            stateTime += Time.deltaTime;
        }else if (stateTime < 0)
        {
            stateMachine.ChangeState(enemy.attackAfterState);
        }
    }
}
