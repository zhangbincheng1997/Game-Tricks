using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    /// <summary>
    /// 摇杆半径
    /// </summary>
    public float JoyStickRadius = 72.0f;
    /// <summary>
    /// 摇杆速度
    /// </summary>
    public float JoyStickResetSpeed = 5.0f;
    /// <summary>
    /// 物体的 Transform 组件
    /// </summary>
    private RectTransform selfTransform;
    /// <summary>
    /// 是否触摸了虚拟摇杆
    /// </summary>
    private bool isTouched = false;
    /// <summary>
    /// 摇杆默认位置
    /// </summary>
    private Vector2 originPosition;
    /// <summary>
    /// 摇杆移动方向
    /// </summary>
    private Vector2 touchedAxis;
    public Vector2 TouchedAxis
    {
        get
        {
            if (touchedAxis.magnitude < JoyStickRadius)
                return touchedAxis.normalized / JoyStickRadius;
            return touchedAxis.normalized;
        }
    }
    /// <summary>
    /// 定义触摸开始事件委托
    /// </summary>
    public delegate void JoyStickTouchBegin(Vector2 vec);
    /// <summary>
    /// 定义触摸过程事件委托
    /// </summary>
    public delegate void JoyStickTouchMove(Vector2 vec);
    /// <summary>
    /// 定义触摸结束事件委托
    /// </summary>
    public delegate void JoyStickTouchEnd();
    /// <summary>
    /// 注册触摸开始事件
    /// </summary>
    public event JoyStickTouchBegin OnJoyStickTouchBegin;
    /// <summary>
    /// 注册触摸过程事件
    /// </summary>
    public event JoyStickTouchMove OnJoyStickTouchMove;
    /// <summary>
    /// 注册触摸结束事件
    /// </summary>
    public event JoyStickTouchEnd OnJoyStickTouchEnd;

    void Start()
    {
        // 初始化虚拟摇杆的默认方向
        selfTransform = this.GetComponent<RectTransform>();
        originPosition = selfTransform.anchoredPosition;
    }

    void Update()
    {
        // 手动触发 OnJoyStickTouchMove 事件
        if (isTouched && touchedAxis.magnitude >= JoyStickRadius)
        {
            if (this.OnJoyStickTouchMove != null)
                this.OnJoyStickTouchMove(TouchedAxis);
        }
        // 松开摇杆后让摇杆回到默认位置
        if (selfTransform.anchoredPosition.magnitude > originPosition.magnitude)
            selfTransform.anchoredPosition -= TouchedAxis * JoyStickResetSpeed * Time.deltaTime;
    }

    // 触摸开始
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouched = true;
        touchedAxis = GetJoyStickAxis(eventData);
        if (this.OnJoyStickTouchBegin != null)
            this.OnJoyStickTouchBegin(TouchedAxis);
    }

    // 触摸结束
    public void OnPointerUp(PointerEventData eventData)
    {
        isTouched = false;
        selfTransform.anchoredPosition = originPosition;
        touchedAxis = Vector2.zero;
        if (this.OnJoyStickTouchEnd != null)
            this.OnJoyStickTouchEnd();
    }

    // 触摸过程
    public void OnDrag(PointerEventData eventData)
    {
        touchedAxis = GetJoyStickAxis(eventData);
        if (this.OnJoyStickTouchMove != null)
            this.OnJoyStickTouchMove(TouchedAxis);
    }

    /// <summary>
    /// 返回摇杆的偏移量
    /// </summary>
    /// <returns>The joy stick axis.</returns>
    /// <param name="eventData">Event data.</param>
    private Vector2 GetJoyStickAxis(PointerEventData eventData)
    {
        // 获取手指位置的世界坐标
        Vector3 worldPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(selfTransform, 
            eventData.position, eventData.pressEventCamera, out worldPosition))
            selfTransform.position = worldPosition;
        // 获取摇杆偏移量
        Vector2 touchAxis = selfTransform.anchoredPosition - originPosition;
        // 摇杆偏移量限制
        if (touchAxis.magnitude >= JoyStickRadius)
        {
            touchAxis = touchAxis.normalized * JoyStickRadius;
            selfTransform.anchoredPosition = touchAxis;
        }
        return touchAxis;
    }
}