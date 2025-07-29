using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTime = player.dashDuration;
        player.skill.dash.CanUseDashClone();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y); // Reset horizontal velocity after dash
    }

    public override void Update()
    {
        base.Update();
        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        if (stateTime < 0)
        {
            stateMachine.ChangeState(player.idleState); // Change to air state after dash
        }
    }
}
