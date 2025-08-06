using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject blackholePrefab; // 黑洞预制体
    [SerializeField] private float maxSize; // 黑洞最大尺寸
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed; // 黑洞缩小速度
    [SerializeField] private float cloneCounts; // 克隆计数
    [SerializeField] private float blackholeDuration;// 黑洞持续时间

    [HideInInspector] public Blackhole_Skill_Controller blackholeController; // 黑洞技能控制器实例

    private void CreateBlackhole()
    {
        GameObject blackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        blackholeController = blackhole.GetComponent<Blackhole_Skill_Controller>();
        if (blackholeController != null)
        {
            blackholeController.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, cloneCounts, blackholeDuration);
        }
        else
        {
            Debug.LogError("Blackhole_Skill_Controller component not found on the blackhole prefab.");
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();
        CreateBlackhole(); // 调用创建黑洞的方法
        AudioManager.instance.PlaySFX(3);
    }

    public bool SkillCompleted()
    {
        if (!blackholeController.isBlackholeActive)
        {
            blackholeController = null; // 清除黑洞控制器实例
            return true;
        }
        return false;
    }

}
