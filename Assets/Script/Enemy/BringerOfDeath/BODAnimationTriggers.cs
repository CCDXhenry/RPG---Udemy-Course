using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BODAnimationTriggers : MonoBehaviour
{
    private Enemy_BringerOfDeath enemy => GetComponentInParent<Enemy_BringerOfDeath>();

    /// <summary>
    /// 动作完成通用方法
    /// </summary>
    private void FinishAnimation()
    {
        enemy.AnimationTrigger();
    }
    private void FinisBlackHand()
    {
        enemy.teleportBlackHandCount--;
        if (enemy.teleportBlackHandCount <= 0)
        {
            enemy.blackHandState.AnimationFinishTrigger();
        }
    }
    private void FinishAttackAfter()
    {
        enemy.attackAfterCount--;
        enemy.stateMachine.currentState.Enter();
        if (enemy.attackAfterCount <= 0)
        {
            enemy.attackAfterState.AnimationFinishTrigger();
        }

    }

    private void FinishAttack()
    {
        enemy.attackState.AnimationFinishTrigger();
    }

    private void FinishTeleportBefore()
    {
        enemy.teleportBeforeState.AnimationFinishTrigger();
    }
    private void FinishTeleportAfter()
    {
        enemy.teleportAfterState.AnimationFinishTrigger();
    }

    private void FinishDead()
    {
        enemy.deadState.AnimationFinishTrigger();
        AudioManager.instance.PlayBGM(0);
        Destroy(enemy.gameObject);
    }

    private void AttackTrigger()
    {
        CloseCounterAttackWindow();
        AudioManager.instance.PlaySFX(1);

        // 攻击方向判断
        var playerTrans = PlayerManager.instance.player.transform;
        float distanceToPlayerX = playerTrans.position.x - enemy.transform.position.x;
        if (distanceToPlayerX > 0 && enemy.facingDirection < 0)
        {
            enemy.Flip();
        }
        else if (distanceToPlayerX < 0 && enemy.facingDirection > 0)
        {
            enemy.Flip();
        }

        //攻击位移
        float offsetX = Random.Range(5f, 8f);
        float offsetY = Random.Range(2f, 4f);
        enemy.rb.velocity = new Vector2(enemy.facingDirection * offsetX, offsetY);

        //触发攻击特效
        if (enemy.TriggerAttackFx())
        {
            offsetX = -2.11f;
            offsetY = 0.32f;
            if (enemy.facingDirection > 0)
            {
                offsetX = 2.126f;
            }
            GameObject attackFx = Instantiate(enemy.AttackFXPrefab, enemy.transform.position + new Vector3(offsetX, offsetY), Quaternion.identity);
            if (enemy.facingDirection > 0)
            {
                attackFx.transform.Rotate(0, 180, 0);

            }
            attackFx.GetComponent<BODAttackFX>().rb.velocity = Vector3.right * enemy.facingDirection * enemy.attackFxSpeed[enemy.bossStage];
        }
        


        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Player player))
            {
                enemy.stats.DoDamage(player.stats);
                AudioManager.instance.PlaySFX(15);

            }
        }
    }
    private void CreateBlackHand()
    {
        //创建黑手
        AudioManager.instance.PlaySFX(16);
        float offsetX = Random.Range(-1f, 1f);
        float offsetY = 0.8f;
        var playerTransPosition = PlayerManager.instance.player.transform.position;
        GameObject blackHand = Instantiate(enemy.blackHandPrefab, playerTransPosition + new Vector3(offsetX, offsetY), Quaternion.identity);

    }

    private void OpenCounterAttackWindow()
    {
        enemy.OpenCounterAttackWindow();
    }
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();


}
