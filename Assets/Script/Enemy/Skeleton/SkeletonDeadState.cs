using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonDeadState : EnemyState 
{
    public Enemy enemy;
    public SkeletonDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        cr.enabled = false;// 禁用碰撞器，防止后续碰撞
        rb.isKinematic = true;// 设置刚体为运动学模式，停止物理模拟
        rb.constraints = RigidbodyConstraints2D.FreezeAll;// 冻结旋转和移动
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

    }
}
