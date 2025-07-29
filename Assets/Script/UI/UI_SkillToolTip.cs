using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillTextName;
    [SerializeField] protected TextMeshProUGUI skillTextDescription;



    public void ShowToolTip(UI_SkillTreeSlot slot)
    {
        skillTextName.text = slot.skillName;
        skillTextDescription.text = slot.description;
        gameObject.SetActive(true);
    }

    public void HideToolTip() {
        gameObject.SetActive(false);
    }
}
