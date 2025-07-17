using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode hotKey; // �ȼ�
    private TextMeshProUGUI hotKeyText; // �ȼ��ı���ʾ

    private Transform myEnemyTransfrom; // ���ڴ洢���˵�Transform
    private Blackhole_Skill_Controller blackholeSkillController; // ���ڴ洢�ڶ����ܿ�����

    public void SetupHotKey(KeyCode _keyCode,Transform _myEnemyTransfrom,Blackhole_Skill_Controller _blackholeSkillController)
    {
        sr = GetComponent<SpriteRenderer>(); // ��ȡ��ǰ�����SpriteRenderer���
        hotKeyText = GetComponentInChildren<TextMeshProUGUI>(); // ��ȡ�Ӷ����е�TextMeshProUGUI���
        hotKey = _keyCode;
        hotKeyText.text = hotKey.ToString(); // ���ȼ���������Ϊ�������ַ�����ʾ
        myEnemyTransfrom = _myEnemyTransfrom; // �洢���˵�Transform
        blackholeSkillController = _blackholeSkillController; // �洢�ڶ����ܿ����� 
    }

    private void Update()
    {
        if (Input.GetKeyDown(hotKey))
        {
            blackholeSkillController.AddEnemyTransfrom(myEnemyTransfrom); // ���úڶ����ܿ������ķ�������ӵ���Transform
            sr.color = Color.clear; // ���ȼ�����ɫ����Ϊ͸��
            hotKeyText.color = Color.clear; // ���ȼ��ı�����ɫ����Ϊ͸��
        }
    }
}
