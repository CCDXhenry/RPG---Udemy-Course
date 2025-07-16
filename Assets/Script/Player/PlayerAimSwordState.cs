using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.sword.DotsActive(true);
        
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, 0);
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }

        //人物朝向鼠标位置
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x > player.transform.position.x && player.facingDirection == -1)
        {
            player.Flip();
        }
        else if(mousePos.x < player.transform.position.x && player.facingDirection == 1)
        {
            player.Flip();
        }
    }
}
