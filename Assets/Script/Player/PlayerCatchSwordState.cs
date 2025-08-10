using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform swordTransform;
    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //AudioManager.instance.PlaySFX(18);
        
        swordTransform = player.sword.transform;
        if(swordTransform.position.x > player.transform.position.x && player.facingDirection == -1)
        {
            player.Flip();
        }
        else if(swordTransform.position.x < player.transform.position.x && player.facingDirection == 1)
        {
            player.Flip();
        }

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDirection, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
