using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour
{
    public bool unlocked = false;
    //需要解锁的技能树槽位
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    //需要锁定的技能树槽位
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    [SerializeField] private Image skillImage;

    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = Color.red;
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
        foreach (var slot in shouldBeUnlocked)
        {
            if (!slot.unlocked)
            {
                Debug.Log("前置技能槽位未解锁");
                return;
            }
        }
        foreach (var slot in shouldBeLocked)
        {
            if (slot.unlocked)
            {
                Debug.Log("分支技能槽位已解锁,无法解锁该技能");
                return;
            }
        }
        unlocked = true;
        skillImage.color = Color.green;
    }
}
