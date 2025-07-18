using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
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

    public virtual bool CanUseSkill(bool _isUseSkill)
    {
        if (cooldownTimer <= 0)
        {
            // Implement skill logic here
            Debug.Log("Skill used!");
            //是否直接使用技能
            if (_isUseSkill)
            {
                UseSkill();
                cooldownTimer = cooldown; // Reset cooldown timer
            }               
            return true;
        }
        
        Debug.Log("Skill is on cooldown!");
        player.StartCoroutine(player.Vibrate(0.2f));
        return false;
    }

    public virtual void UseSkill()
    {

    }

}
