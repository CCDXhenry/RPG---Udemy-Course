using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class BODDeadState : EnemyState
{
    private Enemy_BringerOfDeath enemy;
    public BODDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
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
