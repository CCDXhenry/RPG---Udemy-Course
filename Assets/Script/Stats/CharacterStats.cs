using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    MaxHealth,
    Armor,
    Evasion
}
public class CharacterStats : MonoBehaviour
{
    public Entity entity;
    public EntityFX fx;
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

    public float damageMultiplier = 1f;//伤害倍率

    public int currentHealth;
    public int oweHealth;//因装备而导致的负数血量,以防穿戴装备回血
    //血条值改变事件
    public System.Action onHealthChanged;
    public System.Action triggerHurtOverlay;
    public bool isDead { get; private set; }
    public bool isDeadZone;

    private bool isInvincible;//无敌状态
    protected virtual void Start()
    {
        entity = GetComponent<Entity>();
        currentHealth = GetMaxHealthValue();
        fx = GetComponent<EntityFX>();
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    /// <summary>
    /// 造成伤害并应用到目标角色的生命值上。
    /// </summary>
    /// <param name="_targetStats">目标角色</param>
    /// <param name="_totalDamage">总伤害值,默认值为0,为基本伤害算法</param>
    /// <param name="facingdir">攻击方向,默认值为0,表示用自身面朝的方向作为攻击方向</param>
    public virtual void DoDamage(CharacterStats _targetStats, int _totalDamage = 0, int _facingdir = 0)
    {
        // 计算目标的闪避率，决定是否命中
        bool isHitSuccessful = CheckEvasion(_targetStats);
        if (_targetStats.isInvincible)
        {
            _targetStats.fx.CreatePopUpTextInfo("Invincible");
            return;
        }
        if (!isHitSuccessful)
        {
            _targetStats.fx.CreatePopUpTextInfo("Evade");
            return;
        }

        // 计算总伤害
        int totalDamage = GetTotalDamage(_targetStats, _totalDamage);

        _targetStats.TakeDamage(totalDamage);

        // 触发受伤效果
        int facingdir = _facingdir != 0 ? _facingdir : entity.facingDirection;
        _targetStats.entity.DamageEffect(Vector2.zero, facingdir);
    }



    private int GetTotalDamage(CharacterStats _targetStats, int _totalDamage)
    {

        int totalDamage = _totalDamage != 0 ? _totalDamage : damage.GetValue() + strength.GetValue();
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

    public void SetInvincible(bool _isInvincible)
    {
        isInvincible = _isInvincible;
    }
    public IEnumerator KeepInvincible(int invincibleFrames)
    {
        isInvincible = true;  // 开启无敌
        Debug.Log("KeepInvincible-True");
        // 等待指定帧数
        for (int i = 0; i < invincibleFrames; i++)
        {
            yield return null; // 等待下一帧
        }
        Debug.Log("KeepInvincible-false");
        isInvincible = false; // 关闭无敌
    }

    /// <summary>
    /// 受到伤害并减少当前生命值。
    /// </summary>
    /// <param name="_damage">伤害值</param>
    public virtual void TakeDamage(int _damage)
    {
        if (_damage > 0)
        {
            //随机伤害倍率
            damageMultiplier = Random.Range(0.9f, 1.2f);
            _damage = (int)(_damage * damageMultiplier);

            //Debug.Log($"{gameObject.name} took {_damage} damage. Current health: {currentHealth}");
            currentHealth -= _damage;
            //触发popupTextInfo

            fx.CreatePopUpTextDamage(_damage, damageMultiplier);

            if (entity.entityType == Entitytype.Player)
            {
                if (damageMultiplier > 1.1f)
                {
                    fx.ScreenShake(fx.shakeDamage[1]);
                }
                else
                {
                    fx.ScreenShake(fx.shakeDamage[0]);
                }
                triggerHurtOverlay?.Invoke();// 通知UI显示受伤效果
            }

            onHealthChanged?.Invoke();// 通知UI更新生命值显示
        }

        if (currentHealth <= 0)
        {
            Die();
        }

    }
    public virtual void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} has died.");
        entity.Die();
        AudioManager.instance.PlaySFX(entity.diedSFX);
    }

    public Stat GetStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.Strength:
                return strength;
            case StatType.Agility:
                return agility;
            case StatType.Intelligence:
                return intelligence;
            case StatType.Vitality:
                return vitality;
            case StatType.Damage:
                return damage;
            case StatType.MaxHealth:
                return maxHealth;
            case StatType.Armor:
                return armor;
            case StatType.Evasion:
                return evasion;
            default:
                Debug.LogError("Unknown stat type: " + statType);
                return null;
        }
    }
}
