using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string actionName;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (actionName == "Jump") UniversalButton.jumpPressed = true;
        if (actionName == "Attack") UniversalButton.attackPressed = true;
        if (actionName == "Dash") UniversalButton.dashPressed = true;
        if (actionName == "CrystalSkill") UniversalButton.crystalSkillPressed = true;
        if (actionName == "BlackholeSkill") UniversalButton.blackholeSkillPressed = true;
        if (actionName == "SwordSkill") UniversalButton.swordSkillPressed = true;
}

    public void OnPointerUp(PointerEventData eventData)
    {
        if (actionName == "Jump") UniversalButton.jumpPressed = false;
        if (actionName == "Attack") UniversalButton.attackPressed = false;
        if (actionName == "Dash") UniversalButton.dashPressed = false;
        if (actionName == "CrystalSkill") UniversalButton.crystalSkillPressed = false;
        if (actionName == "BlackholeSkill") UniversalButton.blackholeSkillPressed = false;
        if (actionName == "SwordSkill") UniversalButton.swordSkillPressed = false;
    }
}
