using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private ItemDrop myDropSystem; // 掉落系统
    [SerializeField] private int level = 1; // 敌人等级

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = 0.3f; // 百分比增益

    [SerializeField] private Stat dropSouls; // 掉落灵魂数

    protected override void Start()
    {
        // 设置掉落灵魂数基础值
        dropSouls.SetBaseValue(100);

        // 根据等级和百分比增益调整属性
        ApplyLevelModifers();

        base.Start();

        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);

        Modify(dropSouls);
    }

    private void Modify(Stat _stat)
    {
        // 根据等级和百分比增益调整属性
        for (int i = 0; i < level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    public override void Die()
    {
        base.Die();

        // 掉落物品
        if (myDropSystem == null)
        {
            myDropSystem = GetComponent<ItemDrop>();
        }
        myDropSystem.GenerateDrops();

        // 掉落灵魂数
        PlayerManager.instance.currentSouls += dropSouls.GetValue();

        Destroy(gameObject, 5f);
    }
}
