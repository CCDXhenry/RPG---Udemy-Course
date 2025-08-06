using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextFx : MonoBehaviour
{
    [SerializeField] private TextMeshPro popText;

    [SerializeField] private float popSpeed = 1f;
    [SerializeField] private float popExistime = 1f;
    [SerializeField] private float popDisappearTime = 0.5f;
    //[SerializeField] private float popDisappearSpeed = 1f;
    private Vector2 popLocalScale;//存储初始大小
    private float sizeMultiplier = 1f;//存储大小乘数,根据伤害值调整大小
    private float poptimer;
    void Start()
    {
        if (popText == null)
        {
            popText = GetComponent<TextMeshPro>();
        }
        poptimer = 0f;

        //随机位置
        transform.position += new Vector3(
        Random.Range(-0.3f, 0.3f),
        0,
        Random.Range(-0.3f, 0.3f));
        popLocalScale = transform.localScale;
    }


    void Update()
    {
        poptimer += Time.deltaTime;

        //缓动上升效果
        float speedMultiplier = Mathf.Lerp(1f, 0.2f, poptimer / popExistime);
        transform.position += Vector3.up * popSpeed * speedMultiplier * Time.deltaTime;

        //添加大小变化
        float scaleFactor = 1f + Mathf.Sin(poptimer * 5f) * 0.1f; // 轻微脉动效果
        transform.localScale = popLocalScale * scaleFactor;

        // 消失阶段
        if (poptimer > popDisappearTime)
        {
            float disappearProgress = (poptimer - popDisappearTime) / (popExistime - popDisappearTime);
            float alpha = Mathf.Lerp(1f, 0f, disappearProgress);
            popText.color = new Color(popText.color.r, popText.color.g, popText.color.b, alpha);
            transform.localScale = popLocalScale * (1f - disappearProgress * 0.7f);
        }
        if (poptimer > popExistime)
        {
            Destroy(gameObject);
        }
    }
    public void SetDamageValue(int damage)
    {
        if (popText != null)
        {
            popText.text = damage.ToString();

            // 根据伤害值调整大小
            sizeMultiplier = 1f + Mathf.Log(damage) * 0.1f;
            //transform.localScale = popLocalScale * sizeMultiplier;
        }
    }

}
