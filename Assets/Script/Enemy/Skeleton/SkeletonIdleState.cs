using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0f, rb.velocity.y); // Set the horizontal velocity to 0 for idle state
        stateTime = enemy.idleTime; // Set the idle time for the skeleton
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(stateTime <= 0f) // Check if the idle time has elapsed
        {
            
            stateMachine.ChangeState(enemy.moveState); // Transition to the move state
        }
    }
}
