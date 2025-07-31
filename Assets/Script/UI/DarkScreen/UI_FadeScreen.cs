using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TriggerFadeOut()
    {
        anim.SetTrigger("fadeOut");
    }

    public void TriggerFadeIn()
    {
        anim.SetTrigger("fadeIn");
    }
}
