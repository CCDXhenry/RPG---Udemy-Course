using System.Collections;
using UnityEngine;

namespace Assets.Script.Player
{
    public class PlayerBlackholeState : PlayerState
    {
        private float originalGravity;
        private float upwardForce; // 向上移动的速度
        private float flyTime; // 飞行时间计数
        public PlayerBlackholeState(PlayerStateMachine stateMachine, global::Player player, string animBoolName) : base(stateMachine, player, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            originalGravity = rb.gravityScale;
            rb.gravityScale = 0; // 禁用重力
            flyTime = 0.5f;// 设置飞行时间
            upwardForce = 15f; // 设置向上移动的速度
            skillIsUsed = false;
            stateTime = flyTime; // 初始化状态时间为飞行时间

            player.isBusy = true; // 设置玩家为忙碌状态
        }

        public override void Exit()
        {
            base.Exit();
            rb.gravityScale = originalGravity;
            player.isBusy = false; // 退出状态时将玩家设置为非忙碌状态
        }

        public override void Update()
        {
            base.Update();
            stateTime -= Time.deltaTime;
            if (stateTime > 0)
            {
                rb.velocity = new Vector2(0, upwardForce);
            }
            else 
            {
                rb.velocity = new Vector2(0, -.1f); // 飞行时间结束后，向下移动一点点
                if(!skillIsUsed)
                    skillIsUsed = player.skill.blackhole.CanUseSkill(true);
            }

            if (skillIsUsed && player.skill.blackhole.SkillCompleted())
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}