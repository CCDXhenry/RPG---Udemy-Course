using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Entity entity;
    [Header("Major stats")]
    [Tooltip("力量,影响物理攻击力")]
    public Stat strength;
    [Tooltip("敏捷,影响攻击速度和闪避率")]
    public Stat agility;
    [Tooltip("智力,影响魔法攻击力和法力值")]
    public Stat intelligence;
    [Tooltip("耐力,影响最大生命值和物理防御力")]
    public Stat vitality;

    [Header("Defensive stats")]
    [Tooltip("攻击力,影响物理伤害输出")]
    public Stat damage;
    [Tooltip("最大生命值,影响角色的生存能力")]
    public Stat maxHealth;
    [Tooltip("护甲值,减少受到的物理伤害")]
    public Stat armor;
    [Tooltip("闪避概率")]
    public Stat evasion;

    public int currentHealth;
    public System.Action onHealthChanged;
    public bool isDead { get; private set; }

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        currentHealth = GetMaxHealthValue();
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    /// <summary>
    /// 造成伤害并应用到目标角色的生命值上。
    /// </summary>
    /// <param name="_targetStats">目标角色</param>
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        // 计算目标的闪避率，决定是否命中
        bool isHitSuccessful = CheckEvasion(_targetStats);
        if (!isHitSuccessful)
        {
            return;
        }
        // 计算总伤害
        int totalDamage = GetTotalDamage(_targetStats);

        _targetStats.TakeDamage(totalDamage);
        _targetStats.entity.DamageEffect(Vector2.zero, entity.facingDirection);
    }

    private int GetTotalDamage(CharacterStats _targetStats)
    {
        int totalDamage = damage.GetValue() + strength.GetValue();
        // 计算目标的护甲值，减少伤害
        int armorValue = _targetStats.armor.GetValue();
        if (armorValue > 0)
        {
            totalDamage = Mathf.Max(totalDamage - armorValue, 0); // 确保伤害不为负
        }

        return totalDamage;
    }

    private bool CheckEvasion(CharacterStats _targetStats)
    {
        // 计算目标的闪避率
        int evasionChance = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (Random.Range(0, 100) < evasionChance)
        {
            Debug.Log($"{_targetStats.gameObject.name} evaded the attack!");
            return false; // 如果闪避成功，则不造成伤害
        }

        return true;
    }

    /// <summary>
    /// 受到伤害并减少当前生命值。
    /// </summary>
    /// <param name="_damage">伤害值</param>
    public virtual void TakeDamage(int _damage)
    {
        //Debug.Log($"{gameObject.name} took {_damage} damage. Current health: {currentHealth}");
        currentHealth -= _damage;
        onHealthChanged?.Invoke();// 通知UI更新生命值显示
        if (currentHealth <= 0)
        {
            Die();
        }

    }
    protected virtual void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} has died.");
        entity.Die();
        
    }
}
