using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    public string statName; // 属性名称
    public TextMeshProUGUI statNameText; // 显示属性名称的文本组件
    public TextMeshProUGUI statValueText; // 显示属性值的文本组件
    public StatType statType; // 属性类型枚举
    private void OnValidate()
    {
        gameObject.name = "StatSlot - " + statName; // 确保游戏对象名称为 StatSlot
        if (statNameText != null)
        {
            statNameText.text = statName; // 更新属性名称文本
        }
    }

    private void Start()
    {
        UpdateStatValueUI(); // 初始化时更新属性值UI
    }
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString(); // 获取属性值并更新文本显示
        }
    }
}
