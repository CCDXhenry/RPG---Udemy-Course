using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODIdleState : BODGroundState
{

    public BODIdleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = Vector3.zero;
        stateTime = enemy.bufferPeriod;// 缓冲时间;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.isBattle)
        {
            //判断缓冲时间,尝试触发瞬移技能
            if (stateTime < 0)
            {
                bool triggerTeleport = Random.Range(0f, 1f) <= enemy.teleportProbability * Time.deltaTime;
                if (triggerTeleport)
                {
                    //触发瞬移技能
                    enemy.teleportEnum = enemy.TeleportProSelect();
                    stateMachine.ChangeState(enemy.teleportBeforeState);
                    Debug.Log("判断缓冲时间,尝试触发瞬移技能,触发成功");
                    return;
                }
            }
            //判断enemy与玩家的距离,大于最小距离就切换至移动状态
            CheckMinDistance();
        }
    }

}

