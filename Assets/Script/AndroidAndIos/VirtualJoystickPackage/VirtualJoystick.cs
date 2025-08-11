using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI")]
    public RectTransform bg;      // 背景圆
    public RectTransform handle;  // 小摇杆

    [Header("Settings")]
    public float moveRadius = 100f; // 摇杆最大移动半径（像素）
    [Range(0f, 1f)]
    public float deadZone = 0.1f;   // 死区（0-1）

    private int pointerId = -1;     // 关联的指针 ID
    private Vector2 inputVector = Vector2.zero;

    public Vector2 Direction => inputVector; // 外部访问方向
    public float Magnitude => inputVector.magnitude;

    void Reset()
    {
        // 便于快速绑定：如果没有设置 bg, 尝试用自身
        if (bg == null) bg = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerId = eventData.pointerId;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pointerId != -1 && eventData.pointerId != pointerId) return; // 忽略其他触点

        Vector2 localPoint;
        // 把屏幕坐标转换为 bg 的本地坐标 (以 bg 的中心为原点)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bg, eventData.position, eventData.pressEventCamera, out localPoint);
        // localPoint 的单位是像素，(0,0) 在 bg 的中心

        Vector2 clamped = Vector2.ClampMagnitude(localPoint, moveRadius);
        handle.anchoredPosition = clamped; // 移动 handle

        // 把 clamped 转换成 (-1,1) 区间
        inputVector = clamped / moveRadius;

        // 死区处理
        if (inputVector.magnitude < deadZone) inputVector = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pointerId != -1 && eventData.pointerId != pointerId) return;
        pointerId = -1;
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}
