using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask,
}


[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects; // 物品效果列表

    [Header("Major stats")]
    [Tooltip("力量,影响物理攻击力")]
    public int strength;
    [Tooltip("敏捷,影响攻击速度和闪避率")]
    public int agility;
    [Tooltip("智力,影响魔法攻击力和法力值")]
    public int intelligence;
    [Tooltip("耐力,影响最大生命值和物理防御力")]
    public int vitality;

    [Header("Defensive stats")]
    [Tooltip("攻击力,影响物理伤害输出")]
    public int damage;
    [Tooltip("最大生命值,影响角色的生存能力")]
    public int maxHealth;
    [Tooltip("护甲值,减少受到的物理伤害")]
    public int armor;
    [Tooltip("闪避概率")]
    public int evasion;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.armor.AddModifier(armor);
        playerStats.damage.AddModifier(damage);
        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.evasion.AddModifier(evasion);
        if (maxHealth != 0 || vitality != 0)
        {
            playerStats.currentHealth += maxHealth + vitality * 5 - playerStats.oweHealth;
            playerStats.oweHealth = 0;
            if (playerStats.currentHealth <= 0)
            {
                playerStats.oweHealth += playerStats.currentHealth;
                playerStats.currentHealth = 1;
            }
            playerStats.onHealthChanged?.Invoke();// 通知UI更新生命值显示
        }
    }


    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.armor.RemoveModifier(armor);
        playerStats.damage.RemoveModifier(damage);
        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.evasion.RemoveModifier(evasion);
        if (maxHealth != 0 || vitality != 0)
        {
            playerStats.currentHealth -= maxHealth + vitality * 5 - playerStats.oweHealth;
            playerStats.oweHealth = 0;
            if (playerStats.currentHealth <= 0)
            {
                playerStats.oweHealth += playerStats.currentHealth;
                playerStats.currentHealth = 1;
            }
            playerStats.onHealthChanged?.Invoke();// 通知UI更新生命值显示
        }
    }
    public void ExecuteItemEffect(Transform enemyTransform)
    {
        foreach (ItemEffect effect in itemEffects)
        {
            effect.ExecuteEffect(enemyTransform);
        }
    }

    public override string GetDescription()
    {
        description.Clear();
        AddStatDescription(strength, "Strength");
        AddStatDescription(agility, "Agility");
        AddStatDescription(intelligence, "Intelligence");
        AddStatDescription(vitality, "Vitality");

        AddStatDescription(damage, "Damage");
        AddStatDescription(maxHealth, "Max Health");
        AddStatDescription(armor, "Armor");
        AddStatDescription(evasion, "Evasion");

        description.AppendLine();
        return description.ToString();
    }

    private void AddStatDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (description.Length > 0)
            {
                description.AppendLine();
            }
            string sign = _value > 0 ? "+" : "";
            description.Append(sign + _value + "  " + _name);
        }
    }
}

