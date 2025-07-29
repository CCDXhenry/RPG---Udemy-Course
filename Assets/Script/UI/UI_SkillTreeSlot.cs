using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;


    public bool unlocked = false;
    //需要解锁的技能树槽位
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    //需要锁定的技能树槽位
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    private Image skillImage;
    public string skillName;
    [TextArea]
    public string description;
    [SerializeField] private int skillPrice;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlotUI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UnlockSkillSlot);
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        skillImage.color = Color.white * 0.5f;
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
        if (!PlayerManager.instance.HaveEnoughCurrency(skillPrice))
        {
            Debug.Log("金币不足,无法解锁");
            return;
        }
        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        ui.skillToolTip.ShowToolTip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
