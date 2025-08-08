using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    

    [SerializeField] private Slider healthBarPlayer;
    [SerializeField] private Slider healthBarBoss;

    [SerializeField] private PlayerStats playerStats;
    public EnemyStats enemyStats;
    public TextMeshProUGUI bossName;

    [SerializeField] private TextMeshProUGUI healthStatPlayer;
    [SerializeField] private TextMeshProUGUI healthStatBoss;

    private SkillManager skillManager;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image crystalImage;

    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private int soulFlowSpeed;// 灵魂改变时动态速度
    private float currentSoulsValue;// 目标灵魂值

    public Image HurtOverlay;// 受伤时显示的图片

    private void Start()
    {

        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //enemyStats = GameObject.Find("Enemy_BringerOfDeath").GetComponent<EnemyStats>();

        //注册血条更新事件
        playerStats.onHealthChanged += () => Update_Health_UI(playerStats, healthBarPlayer, healthStatPlayer);
        //注册受伤事件
        playerStats.triggerHurtOverlay += () => HurtOverlay.GetComponent<UI_HurtOverlay>().ShowHurtEffect();
        //enemyStats.onHealthChanged += () => Update_Health_UI(enemyStats, healthBarBoss, healthStatBoss);

        //初始化血条
        playerStats.onHealthChanged.Invoke();
        //enemyStats.onHealthChanged.Invoke();

        //注册技能冷却UI事件
        skillManager = SkillManager.instance;
        skillManager.dash.OnSkillUiUpdated += () => SetCooldownOf(dashImage);
        skillManager.blackhole.OnSkillUiUpdated += () => SetCooldownOf(blackholeImage);
        skillManager.sword.OnSkillUiUpdated += () => SetCooldownOf(swordImage);
        skillManager.crystal.OnSkillUiUpdated += () => SetCooldownOf(crystalImage);

        //初始化冷却时间
        dashImage.fillAmount = 0;
        blackholeImage.fillAmount = 0;
        swordImage.fillAmount = 0;
        crystalImage.fillAmount = 0;

        //初始化灵魂UI变更速度
        if (soulFlowSpeed == 0)
        {
            soulFlowSpeed = 10;
        }
    }


    private void Update()
    {
        //更新灵魂数UI
        UpdateSoulUI();

        CheckCooldownOf(dashImage, skillManager.dash.cooldown);
        CheckCooldownOf(blackholeImage, skillManager.blackhole.cooldown);
        CheckCooldownOf(swordImage, skillManager.sword.cooldown);
        CheckCooldownOf(crystalImage, skillManager.crystal.cooldown);

        //判断Boss血条是否显示
        if (enemyStats == null)
        {
            if (healthBarBoss.gameObject.activeSelf)
                healthBarBoss.gameObject.SetActive(false);
        }
        else
        {
            if (!healthBarBoss.gameObject.activeSelf)
            {
                healthBarBoss.gameObject.SetActive(true);
                enemyStats.onHealthChanged += () => Update_Health_UI(enemyStats, healthBarBoss, healthStatBoss);
                enemyStats.onHealthChanged.Invoke();
            }
                
        }
    }
    private void GetBossInfo()
    {

    }
    private void UpdateSoulUI()
    {
        float targetSoulValue = PlayerManager.instance.GetCurrentSouls();
        if (currentSoulsValue != targetSoulValue)
        {
            currentSoulsValue = Mathf.Lerp(currentSoulsValue, targetSoulValue, Time.deltaTime * soulFlowSpeed);
        }
        if (Mathf.Abs(targetSoulValue - currentSoulsValue) < 0.5f)
        {
            currentSoulsValue = targetSoulValue;
        }

        currentSouls.text = "x" + (int)currentSoulsValue;
    }

    private void Update_Health_UI(CharacterStats characterStats, Slider healthBar, TextMeshProUGUI healthStat)
    {
        healthBar.maxValue = characterStats.GetMaxHealthValue();
        healthBar.value = characterStats.currentHealth;

        healthStat.text = healthBar.value + "/" + healthBar.maxValue;
        float healthPercentage = healthBar.value / healthBar.maxValue;
        if (healthPercentage > .8f)
        {
            healthStat.color = Color.green;
            characterStats.entity.bossStage = 0;
        }
        else if (healthPercentage > .4f)
        {
            healthStat.color = Color.yellow;
            characterStats.entity.bossStage = 1;
        }
        else
        {
            healthStat.color = Color.red;
            characterStats.entity.bossStage = 2;
        }
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }
    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        skillManager.dash.OnSkillUiUpdated -= () => SetCooldownOf(dashImage);
        skillManager.blackhole.OnSkillUiUpdated -= () => SetCooldownOf(blackholeImage);
        skillManager.sword.OnSkillUiUpdated -= () => SetCooldownOf(swordImage);
        skillManager.crystal.OnSkillUiUpdated -= () => SetCooldownOf(crystalImage);
        playerStats.onHealthChanged -= () => Update_Health_UI(playerStats, healthBarPlayer, healthStatPlayer);
        playerStats.triggerHurtOverlay -= () => HurtOverlay.GetComponent<UI_HurtOverlay>().ShowHurtEffect();
        if (enemyStats != null)
            enemyStats.onHealthChanged -= () => Update_Health_UI(enemyStats, healthBarBoss, healthStatBoss);
    }
}
