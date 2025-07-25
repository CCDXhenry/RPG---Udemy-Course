using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected string animBoolName;
    protected float xInput;
    protected float yInput;
    public float stateTime;
    public bool triggerCalled;
    protected Rigidbody2D rb;
    protected bool skillIsUsed = false;
    protected CapsuleCollider2D cr;
    public PlayerState(PlayerStateMachine stateMachine, Player player, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.player = player;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        cr = player.cr;
        triggerCalled = false;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        
    }
    public virtual void Update()
    {
       
       xInput = Input.GetAxisRaw("Horizontal");
       yInput = Input.GetAxisRaw("Vertical");
       player.anim.SetFloat("yVelocity", rb.velocity.y);
       stateTime -= Time.deltaTime;
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
