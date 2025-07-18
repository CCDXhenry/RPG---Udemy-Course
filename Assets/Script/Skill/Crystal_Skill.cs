using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab; // 水晶预制体
    private GameObject currentCrystal;
    [SerializeField] private float crystalDuration; // 水晶持续时间
    [SerializeField] private float maxDistance; // 水晶最大距离
    [SerializeField] private bool canGrow; // 是否允许水晶生长
    [SerializeField] private float growSpeed; // 水晶生长速度
    [SerializeField] private float maxSize; // 水晶最大大小

    private Crystal_Skill_Controller crystalController; // 水晶技能控制器实例
    public override void UseSkill()
    {
        base.UseSkill();
        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            crystalController = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            crystalController.SetupCrystal(currentCrystal, crystalDuration, maxDistance, canGrow, growSpeed, maxSize);
        }
        else
        {
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position; // 将玩家位置设置为水晶位置
            currentCrystal.transform.position = playerPos; // 将水晶位置设置为玩家原位置

            crystalController.isExploding = true; // 设置水晶为爆炸状态
            //Destroy(currentCrystal); // 销毁水晶
        }
    }
    public override bool CanUseSkill(bool _isUseSkill)
    {
        if (currentCrystal)
        {
            UseSkill();
            return true; // 如果水晶存在，直接使用技能
        }
        return base.CanUseSkill(_isUseSkill);
    }
}
