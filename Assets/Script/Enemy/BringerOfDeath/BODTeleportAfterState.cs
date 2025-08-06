using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODTeleportAfterState : EnemyState
{
    private Enemy_BringerOfDeath enemy;
    public BODTeleportAfterState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = Vector3.zero;
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
            if (enemy.teleportEnum == BODTeleportEnum.attack)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

    }
}
