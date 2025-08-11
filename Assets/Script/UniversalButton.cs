using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UniversalButton
{
    // 由 UI 按钮调用

    public static bool attackPressed;//攻击
    public static bool jumpPressed;//跳跃
    public static bool dashPressed;//冲刺

    public static bool crystalSkillPressed;//水晶技能
    public static bool blackholeSkillPressed;//黑洞技能
    public static bool swordSkillPressed;//剑技能
    

    public static bool IsAttackPressed()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetKeyDown(KeyCode.Mouse0);
#elif UNITY_ANDROID || UNITY_IOS
        return jumpPressed;
#else
        return false;
#endif
    }
    public static bool IsJumpPressed()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetKeyDown(KeyCode.Space);
#elif UNITY_ANDROID || UNITY_IOS
        return jumpPressed;
#else
        return false;
#endif
    }

    public static bool IsDashPressed()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetKeyDown(KeyCode.LeftShift);
#elif UNITY_ANDROID || UNITY_IOS
        return dashPressed;
#else
        return false;
#endif
    }

    public static bool IsCrystalSkillPressed()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetKeyDown(KeyCode.F);
#elif UNITY_ANDROID || UNITY_IOS
        return crystalSkillPressed;
#else
        return false;
#endif
    }

    public static bool IsBlackholeSkillPressed() {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetKeyDown(KeyCode.R);
#elif UNITY_ANDROID || UNITY_IOS
        return blackholeSkillPressed;
#else
        return false;
#endif   
    }

}
