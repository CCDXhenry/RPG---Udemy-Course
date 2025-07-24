using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private CharacterStats stats; // Reference to CharacterStats to get health information
    private Slider slider;
    void Start()
    {
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        stats = GetComponentInParent<CharacterStats>();
        slider = GetComponentInChildren<Slider>();

        entity.onFlipped += FlipUI; // Subscribe to the onFlipped event
        stats.onHealthChanged += Update_Health_UI; // Subscribe to health changes

        Update_Health_UI(); // Initialize the slider value
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FlipUI()
    {
        rectTransform.Rotate(0, 180, 0);
    }


    private void Update_Health_UI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    private void OnDestroy()
    {
        if (entity != null)
        {
            entity.onFlipped -= FlipUI; // Unsubscribe from the event to avoid memory leaks
        }
    }
}
