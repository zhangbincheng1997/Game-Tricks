using UnityEngine;

public class JSMove : MonoBehaviour
{
    public float speed = 2.0f;
    private JoyStick js;

    void Start()
    {
        js = GameObject.FindObjectOfType<JoyStick>();
        js.OnJoyStickTouchBegin += OnJoyStickBegin;
        js.OnJoyStickTouchMove += OnJoyStickMove;
        js.OnJoyStickTouchEnd += OnJoyStickEnd;
    }

    void OnJoyStickBegin(Vector2 vec)
    {
        Debug.Log("开始触摸虚拟摇杆");
    }

    void OnJoyStickMove(Vector2 vec)
    {
        Debug.Log("正在移动虚拟摇杆");
        // 角色朝向
        transform.rotation = Quaternion.LookRotation(new Vector3(vec.x, 0, vec.y));
        // 角色移动
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnJoyStickEnd()
    {
        Debug.Log("触摸移动摇杆结束");
    }
}