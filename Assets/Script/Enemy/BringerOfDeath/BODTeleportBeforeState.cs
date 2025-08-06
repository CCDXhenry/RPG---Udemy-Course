using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODTeleportBeforeState : EnemyState
{
    private Enemy_BringerOfDeath enemy;
    public BODTeleportBeforeState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_BringerOfDeath enemy) : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = Vector3.zero;
        AudioManager.instance.PlaySFX(16);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            //触发瞬移攻击模式,随机移动到角色位置前后
            if (enemy.teleportEnum == BODTeleportEnum.attack)
            {
                var player = PlayerManager.instance.player;
                float teleportDir = Random.Range(0,2) == 0 ? -1 : 1;
                float offsetX = Random.Range(1f, 3f);
                //计算瞬移后的安全坐标
                var teleportTrans = enemy.SafeTeleport(player.transform.position + new Vector3(player.facingDirection * teleportDir * offsetX, 0));
                enemy.transform.position = teleportTrans;
                stateMachine.ChangeState(enemy.teleportAfterState);
            }
        }
        
    }
}
