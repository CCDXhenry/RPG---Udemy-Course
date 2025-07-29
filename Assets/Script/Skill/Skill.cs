using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    public float cooldown;
    protected float cooldownTimer;

    //public bool skillUiUpdated = false;//用来触发技能ui的更新

    public event Action OnSkillUiUpdated;//用来触发技能ui的更新事件
    protected Player player;


    protected virtual void Start()
    {
        cooldownTimer = 0;
        player = PlayerManager.instance.player;
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;

    }
    public virtual void TriggerUiUpdate()
    {
        OnSkillUiUpdated?.Invoke();
    }
    public virtual bool CanUseSkill(bool _isUseSkill)
    {
        if (cooldownTimer <= 0)
        {
            //是否直接使用技能
            if (_isUseSkill)
            {
                UseSkill();
                cooldownTimer = cooldown; // Reset cooldown timer
            }
            return true;
        }

        Debug.Log("Skill is on cooldown!");
        if (_isUseSkill)
            player.StartCoroutine(player.Vibrate(0.2f));
        return false;
    }

    public virtual void UseSkill()
    {
        //skillUiUpdated = true;
        TriggerUiUpdate();
    }

}
