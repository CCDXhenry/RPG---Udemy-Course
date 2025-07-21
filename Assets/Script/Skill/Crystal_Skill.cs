using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CrystalTags
{
    Switchover, // 切换水晶
    Move, // 移动水晶
}

public class Crystal_Skill : Skill
{
    [Header("Info")]
    public CrystalTags crystalTag = CrystalTags.Switchover; // 水晶技能标签，用于区分不同的水晶技能类型
    [SerializeField] private GameObject crystalPrefab; // 水晶预制体
    [SerializeField] private float crystalDuration; // 水晶持续时间
    [SerializeField] private float maxDistance; // 水晶最大距离
    [SerializeField] private bool canGrow; // 是否允许水晶生长
    [SerializeField] private float growSpeed; // 水晶生长速度
    [SerializeField] private float maxSize; // 水晶最大大小

    //private Dictionary<CrystalTags, float> CrystalCooldown = new Dictionary<CrystalTags, float>()
    //{
    //    { CrystalTags.Switchover, 5f }, // 切换水晶冷却时间
    //    { CrystalTags.Move, 10f } // 移动水晶冷却时间
    //};
    [Header("Switchover Crystal Skill")]
    private GameObject currentSwitchoverCrystal; // 当前水晶实例
    private Crystal_Skill_Controller switchoverCrystalController; // 水晶技能控制器实例

    [Header("Move Crystal Skill")]
    [SerializeField] private float moveSpeed; // 移动水晶技能速度
    [SerializeField] private float moveDistance; // 移动水晶技能距离
    [SerializeField] private Vector2 moveDirection; // 移动水晶技能方向

    [SerializeField] private int maxMoveCrystals; // 最大移动水晶数量
    [SerializeField] private List<GameObject> moveCrystalList; // 存储移动水晶的列表
    [SerializeField] private GameObject currentMoveCrystal; // 当前移动水晶实例
    [SerializeField] private float moveSkillCooldown; // 移动技能填充冷却时间
    private float moveSkillCooldownTimer; // 移动技能冷却计时器



    public override void UseSkill()
    {
        base.UseSkill();


        if (crystalTag == CrystalTags.Switchover)
        {
            ActivateSwitchoverCrystal();
        }

        if (crystalTag == CrystalTags.Move)
        {
            if (moveCrystalList.Count > 0)
            {
                //创建水晶
                currentMoveCrystal = moveCrystalList[moveCrystalList.Count - 1];
                moveCrystalList.RemoveAt(moveCrystalList.Count - 1);
                currentMoveCrystal = Instantiate(currentMoveCrystal, player.transform.position, Quaternion.identity);

                Crystal_Skill_Controller crystalController = currentMoveCrystal.GetComponent<Crystal_Skill_Controller>();
                crystalController.GetComponent<Crystal_Skill_Controller>().SetupCrystal(
                    crystalTag, currentMoveCrystal, crystalDuration, moveDistance, canGrow, growSpeed, maxSize);// 设置水晶技能参数

                moveDirection = SkillManager.instance.sword.AimDirection().normalized; // 获取鼠标的朝向并归一化

                crystalController.SetupMoveCrystal(moveSpeed, moveDistance, moveDirection); // 设置移动水晶技能参数

                moveSkillCooldownTimer = moveSkillCooldown;
            }
        }
    }

    private void ActivateSwitchoverCrystal()
    {
        if (currentSwitchoverCrystal == null)
        {
            currentSwitchoverCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            switchoverCrystalController = currentSwitchoverCrystal.GetComponent<Crystal_Skill_Controller>();
            switchoverCrystalController.SetupCrystal(crystalTag,currentSwitchoverCrystal, crystalDuration, maxDistance, canGrow, growSpeed, maxSize);
        }
        else
        {
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentSwitchoverCrystal.transform.position; // 将玩家位置设置为水晶位置
            currentSwitchoverCrystal.transform.position = playerPos; // 将水晶位置设置为玩家原位置

            switchoverCrystalController.isExploding = true; // 设置水晶为爆炸状态
        }
    }

    protected override void Start()
    {
        base.Start();
        moveSkillCooldownTimer = 0f; // 初始化移动技能冷却计时器
        moveCrystalList = new List<GameObject>(maxMoveCrystals); // 初始化移动水晶列表
        ResetMoveCrystalList();
    }


    private void ResetMoveCrystalList() //重置移动水晶列表
    {
        int count = moveCrystalList.Count;
        for (int i = count; i < maxMoveCrystals; i++)
        {
            moveCrystalList.Add(crystalPrefab);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (crystalTag == CrystalTags.Move)
        {
            moveSkillCooldownTimer -= Time.deltaTime; // 更新移动技能冷却计时器
            if (moveSkillCooldownTimer <= 0f)
            {
                ResetMoveCrystalList(); // 重置移动水晶列表
                moveSkillCooldownTimer = moveSkillCooldown; // 重置冷却计时器
            }
        }
    }

    public override bool CanUseSkill(bool _isUseSkill)
    {
        if (crystalTag == CrystalTags.Switchover && currentSwitchoverCrystal)
        {
            UseSkill();
            return true; // 如果水晶存在，直接使用技能
        }
        if (crystalTag == CrystalTags.Move && moveCrystalList.Count > 0)
        {
            UseSkill();
            return true;// 如果有可用的移动水晶，直接使用技能
        }
        return base.CanUseSkill(_isUseSkill);
    }
}
