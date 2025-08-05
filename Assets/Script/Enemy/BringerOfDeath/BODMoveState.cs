using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODMoveState : BODGroundState
{
    public BODMoveState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
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

        //enemy移动
        if (enemy.isBattle)
        {
            enemy.rb.velocity = new Vector3(enemy.moveSpeed * enemy.facingDirection, enemy.rb.velocity.y);
        }


    }
}
