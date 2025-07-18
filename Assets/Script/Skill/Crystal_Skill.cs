using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab; // ˮ��Ԥ����
    private GameObject currentCrystal;
    [SerializeField] private float crystalDuration; // ˮ������ʱ��
    [SerializeField] private float maxDistance; // ˮ��������
    [SerializeField] private bool canGrow; // �Ƿ�����ˮ������
    [SerializeField] private float growSpeed; // ˮ�������ٶ�
    [SerializeField] private float maxSize; // ˮ������С

    private Crystal_Skill_Controller crystalController; // ˮ�����ܿ�����ʵ��
    public override void UseSkill()
    {
        base.UseSkill();
        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            crystalController = currentCrystal.GetComponent<Crystal_Skill_Controller>();
            crystalController.SetupCrystal(currentCrystal, crystalDuration, maxDistance, canGrow, growSpeed, maxSize);
        }
        else
        {
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position; // �����λ������Ϊˮ��λ��
            currentCrystal.transform.position = playerPos; // ��ˮ��λ������Ϊ���ԭλ��

            crystalController.isExploding = true; // ����ˮ��Ϊ��ը״̬
            //Destroy(currentCrystal); // ����ˮ��
        }
    }
    public override bool CanUseSkill(bool _isUseSkill)
    {
        if (currentCrystal)
        {
            UseSkill();
            return true; // ���ˮ�����ڣ�ֱ��ʹ�ü���
        }
        return base.CanUseSkill(_isUseSkill);
    }
}
