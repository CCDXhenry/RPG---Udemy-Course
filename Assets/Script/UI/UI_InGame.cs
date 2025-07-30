using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI healthStat;
    private SkillManager skillManager;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private TextMeshProUGUI currencyText;

    private void Start()
    {


        if (playerStats == null)
        {
            playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        }
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<Slider>();
        }
        if (healthStat == null)
        {
            healthStat = GetComponentInChildren<TextMeshProUGUI>();
        }

        //注册血条更新事件
        playerStats.onHealthChanged += Update_Health_UI;
        playerStats.onHealthChanged.Invoke();//初始化血条

        //注册技能冷却UI事件
        skillManager = SkillManager.instance;
        skillManager.dash.OnSkillUiUpdated += () => SetCooldownOf(dashImage);
        skillManager.blackhole.OnSkillUiUpdated += () => SetCooldownOf(blackholeImage);

        //初始化冷却时间
        dashImage.fillAmount = 0;
        blackholeImage.fillAmount = 0;
    }

    private void Update()
    {
        //if (skillManager.dash.skillUiUpdated)
        //{
        //    SetCooldownOf(dashImage);
        //    skillManager.dash.skillUiUpdated = false;
        //}
        currencyText.text = "x" + PlayerManager.instance.GetCurrency().ToString("#,#");


        CheckCooldownOf(dashImage, skillManager.dash.cooldown);
        CheckCooldownOf(blackholeImage, skillManager.blackhole.cooldown);
    }

    private void Update_Health_UI()
    {
        healthBar.maxValue = playerStats.GetMaxHealthValue();
        healthBar.value = playerStats.currentHealth;
        healthStat.text = healthBar.value + "/" + healthBar.maxValue;
        float healthPercentage = healthBar.value / healthBar.maxValue;
        if (healthPercentage > .8f)
        {
            healthStat.color = Color.green;
        }
        else if (healthPercentage > .4f)
        {
            healthStat.color = Color.yellow;
        }
        else
        {
            healthStat.color = Color.red;
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
        playerStats.onHealthChanged -= Update_Health_UI;
    }
}
