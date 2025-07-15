using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (xInput * player.facingDirection <= 0)
                player.Flip();
            stateMachine.ChangeState(player.jumpState);
            return;
        }
        if (xInput * player.facingDirection >= 0 && !player.IsGroundedDetected())
        {
            if (xInput * player.facingDirection > 0)
                player.SetVelocity(0f, rb.velocity.y * .4f);
            else
                player.SetVelocity(0f, rb.velocity.y * .8f);
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}