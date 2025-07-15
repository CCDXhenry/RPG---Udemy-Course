using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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
        if(xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else 
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        }

    }
}
