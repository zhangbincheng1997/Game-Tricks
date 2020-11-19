using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("拉近的最大值")]
    public float minDistance = 2.0f;
    [Header("拉远的最大值")]
    public float maxDistance = 10.0f;
    [Header("向上最大值")]
    public float upAngle = 90.0f;
    [Header("向下最大值")]
    public float downAngle = 30.0f;
    [Header("向左最大值")]
    public float leftAngle = 60.0f;
    [Header("向右最大值")]
    public float rightAngle = 60.0f;
    [Header("控制缩放的速度")]
    public float scrollSpeed = 5;
    [Header("控制旋转的速度")]
    public float rotateSpeed = 5;

    private Transform player;
    private Vector3 offsetPosition;  // 初始镜头和玩家位置的偏移量

    private const string playerTag = "Player";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        offsetPosition = transform.position - player.position;
    }

    void Update()
    {
        transform.position = offsetPosition + player.position;

        ScrollView();
        RotateView();
    }

    // 控制镜头的拉近拉远
    void ScrollView()
    {
        float distance = offsetPosition.magnitude;
        distance += Input.GetAxis("Mouse ScrollWheel") * -scrollSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        offsetPosition = offsetPosition.normalized * distance;
    }

    // 控制镜头的左右上下
    void RotateView()
    {
        if (Input.GetMouseButton(1))
        {

            Vector3 originalPos = transform.position;  // 记录上一帧镜头位置
            Quaternion originalRotation = transform.rotation;  // 记录上一帧镜头旋转

            // 上下
            transform.RotateAround(player.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;
            if (x > upAngle && x < 360 - downAngle)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }

            // 左右
            transform.RotateAround(player.position, player.up, rotateSpeed * Input.GetAxis("Mouse X"));
            float y = transform.eulerAngles.y;
            if (y > leftAngle && y < 360 - rightAngle)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }

            offsetPosition = transform.position - player.position;
        }
    }
}
