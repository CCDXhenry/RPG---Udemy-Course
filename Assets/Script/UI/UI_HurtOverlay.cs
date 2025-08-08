using UnityEngine;
using UnityEngine.UI;

public class UI_HurtOverlay : MonoBehaviour
{
    public Image redOverlayImage;     // 拖入你的 UI Image
    public float fadeSpeed = 2f;      // 淡出速度
    public float targetAlphaValue = 0.3f; // 透明度
    private float targetAlpha = 0f;   // 目标透明度
    

    void Update()
    {
        // 平滑渐变 Alpha
        Color color = redOverlayImage.color;
        color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * fadeSpeed);
        redOverlayImage.color = color;
    }

    // 受伤时调用这个函数
    public void ShowHurtEffect()
    {
        targetAlpha = targetAlphaValue;            // 设置红色透明度
        Invoke(nameof(FadeOut), 0.1f); // 0.1秒后开始淡出
    }

    void FadeOut()
    {
        targetAlpha = 0f;
    }
}
