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
        if (!CheckMinDistance())
        {
            var rand = Random.Range(0f, 1f);
            if (rand <= 0.5f)
            {
                enemy.Flip();
            }
        }

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
            CheckMinDistance();
            enemy.rb.velocity = new Vector3(enemy.moveSpeed * enemy.facingDirection, enemy.rb.velocity.y);
        }


    }

    
}
