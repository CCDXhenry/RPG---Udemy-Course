using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    // Start is called before the first frame update
    public PlayerAirState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySFX(23);
    }

    public override void Update()
    {
        base.Update();
        if (player.IsGroundedDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if (player.IsWallDetected() && xInput * player.facingDirection >= 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        player.SetVelocity(xInput * player.moveSpeed * 0.8f, rb.velocity.y);
    }
}
