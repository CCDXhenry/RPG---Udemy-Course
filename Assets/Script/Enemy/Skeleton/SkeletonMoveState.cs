using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
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
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, enemy.rb.velocity.y);

        if (!enemy.IsGroundedDetected() || enemy.IsWallDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState); // Transition to the idle state if no ground or wall detected
        }
    }

}
