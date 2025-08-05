using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected string animBoolName;
    public float stateTime;
    public bool triggerCalled;
    protected Rigidbody2D rb;
    protected CapsuleCollider2D cr;
    public EnemyState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.enemyBase = enemyBase;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName, true);
        rb = enemyBase.rb;
        cr = enemyBase.cr;
    }
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
    }
    public virtual void Update()
    {
        stateTime -= Time.deltaTime;
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;

    }

}
