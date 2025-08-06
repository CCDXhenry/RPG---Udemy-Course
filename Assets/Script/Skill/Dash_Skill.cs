using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked = false;
    public UI_SkillTreeSlot dashUnlockButton;

    [Header("DashClone")]
    public bool dashCloneUnlocked = false;
    public UI_SkillTreeSlot dashCloneUnlockButton;

    [Header("DashCloneMore")]
    public bool dashCloneMorelocked = false;
    public UI_SkillTreeSlot DashCloneMoreUnlockButton;


    public override void UseSkill()
    {
        base.UseSkill();
        Debug.Log("dash used");
        AudioManager.instance.PlaySFX(21);
    }
    private void Awake()
    {
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        dashCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDashClone);
        DashCloneMoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDashCloneMore);
    }
    protected override void Start()
    {
        base.Start();
        
    }
    public override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockDash();
        UnlockDashClone();
        UnlockDashCloneMore();
    }

    public void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
        {
            dashUnlocked = true;
        }
    }

    public void UnlockDashClone()
    {
        if (dashCloneUnlockButton.unlocked)
        {
            dashCloneUnlocked = true;
        }
    }

    public void UnlockDashCloneMore()
    {
        if (dashCloneUnlockButton.unlocked)
        {
            dashCloneMorelocked = true;
        }
    }

    public bool CanUseDash()
    {
        if (dashUnlocked)
        {
            return true;
        }
        else
        {
            Debug.Log("Dash未解锁");
            return false;
        }
    }
    public bool CanUseDashClone()
    {
        if (dashCloneUnlocked)
        {
            player.skill.clone.CreateClone(player.transform, Vector3.zero);
            return true;
        }
        else
        {
            Debug.Log("DashClone未解锁");
            return false;
        }
    }
    public bool CanUseDashCloneMore()
    {
        if (dashCloneMorelocked)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override bool CanUseSkill(bool _isUseSkill)
    {
        if (!CanUseDash())
        {
            return false;
        }
        return base.CanUseSkill(_isUseSkill);
    }
}
