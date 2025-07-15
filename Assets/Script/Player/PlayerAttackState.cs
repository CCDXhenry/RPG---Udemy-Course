using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    // Start is called before the first frame update
    private float lastTimeAttacked;
    private float attackCooldown = 1f;
    public PlayerAttackState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (Time.time >= lastTimeAttacked + attackCooldown)
        {
            player.comboCounts = 0;
        }
        
        player.anim.SetInteger("comboCounts", player.comboCounts);

        stateTime = 0.1f;
        player.SetVelocity(player.facingDirection * player.attackMovement[player.comboCounts].x, player.attackMovement[player.comboCounts].y);

    }

    public override void Exit()
    {
        base.Exit();
        player.comboCounts = (player.comboCounts + 1) % 3; // Cycle through 0, 1, 2
        lastTimeAttacked = Time.time;
        player.StartCoroutine("BusyFor", 0.13f);
    }

    public override void Update()
    {
        base.Update();
        
        if (stateTime < 0)
        {
            player.SetVelocity(0f, 0f);
        }
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
