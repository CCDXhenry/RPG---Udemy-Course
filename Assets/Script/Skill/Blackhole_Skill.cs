using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject blackholePrefab; // �ڶ�Ԥ����
    [SerializeField] private float maxSize; // �ڶ����ߴ�
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed; // �ڶ���С�ٶ�
    [SerializeField] private float cloneCounts; // ��¡����
    [SerializeField] private float blackholeDuration;// �ڶ�����ʱ��

    [HideInInspector] public Blackhole_Skill_Controller blackholeController; // �ڶ����ܿ�����ʵ��

    private void CreateBlackhole()
    {
        base.UseSkill();
        GameObject blackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        blackholeController = blackhole.GetComponent<Blackhole_Skill_Controller>();
        if (blackholeController != null)
        {
            blackholeController.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, cloneCounts, blackholeDuration);
        }
        else
        {
            Debug.LogError("Blackhole_Skill_Controller component not found on the blackhole prefab.");
        }
    }

    public override void UseSkill()
    {
        base.UseSkill();
        CreateBlackhole(); // ���ô����ڶ��ķ���
    }

    public bool SkillCompleted()
    {
        if (!blackholeController.isBlackholeActive)
        {
            blackholeController = null; // ����ڶ�������ʵ��
            return true;
        }
        return false;
    }

}
