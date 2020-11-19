using UnityEngine;

public class LerpRun : MonoBehaviour
{
    // 玩家移动速度
    public float speed = 5.0f;
    // 玩家平滑速度
    public float smoothing = 2.0f;

    // 动画组件
    private Animator anim;
    // 最小距离
    private float minDistance = 0.01f;
    // 是否正在运动
    private bool isRun = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 向左移动
        if (h < 0)
        {
            // 人物Y轴翻转
            transform.eulerAngles = new Vector3(0, 180.0f, 0);
        }
        // 向右移动
        else if (h > 0)
        {
            // 人物Y轴翻转 
            transform.eulerAngles = Vector3.zero;
        }

        if (Mathf.Abs(h) >= minDistance || Mathf.Abs(v) >= minDistance)
        {
            // 获取移动方向
            Vector3 tarDir = new Vector3(h, v, 0);
            // 控制玩家移动
            /*
                Vector3.Lerp(from, to, smoothing) 线性插值
                返回t是在 [0...1]之间
                当t = 0时，返回from
                当t = 1时，返回to
                当t = 0.5 返回from和to的平均数。
                公式 from + (to - from) * smoothing
            */
            transform.position = Vector3.Lerp(transform.position,
                transform.position + tarDir * speed, smoothing * Time.deltaTime);

            if (!isRun)
            {
                isRun = true;
                // 控制玩家运动动画
                anim.SetBool("Run", true);
            }
        }
        else
        {
            isRun = false;
            // 播放玩家休息动画
            anim.SetBool("Run", false);
        }
    }
}
