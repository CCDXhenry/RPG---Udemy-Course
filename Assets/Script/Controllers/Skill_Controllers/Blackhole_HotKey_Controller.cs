using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode hotKey; // 热键
    private TextMeshProUGUI hotKeyText; // 热键文本显示

    private Transform myEnemyTransfrom; // 用于存储敌人的Transform
    private Blackhole_Skill_Controller blackholeSkillController; // 用于存储黑洞技能控制器

    public void SetupHotKey(KeyCode _keyCode,Transform _myEnemyTransfrom,Blackhole_Skill_Controller _blackholeSkillController)
    {
        sr = GetComponent<SpriteRenderer>(); // 获取当前对象的SpriteRenderer组件
        hotKeyText = GetComponentInChildren<TextMeshProUGUI>(); // 获取子对象中的TextMeshProUGUI组件
        hotKey = _keyCode;
        hotKeyText.text = hotKey.ToString(); // 将热键名称设置为按键的字符串表示
        myEnemyTransfrom = _myEnemyTransfrom; // 存储敌人的Transform
        blackholeSkillController = _blackholeSkillController; // 存储黑洞技能控制器 
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotKey))
        {
            blackholeSkillController.AddEnemyTransfrom(myEnemyTransfrom); // 调用黑洞技能控制器的方法，添加敌人Transform
            sr.color = Color.clear; // 将热键的颜色设置为透明
            hotKeyText.color = Color.clear; // 将热键文本的颜色设置为透明
        }
    }
}
