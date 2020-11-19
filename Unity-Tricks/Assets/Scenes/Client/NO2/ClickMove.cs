using UnityEngine;

public class ClickMove : MonoBehaviour
{
    public float speed = 4.0f;

    private CharacterController mController;
    private Vector3 targetPosition;
    private bool isReach = true;
    private float minDistance = 0.1f;  // 当小于这个距离的时候 判端为 达到目标

    private const string groundTag = "Ground";  // Ground 标签

    void Start()
    {
        mController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 左键按下
        {
            /*
                Camera.main.ScreenPointToRay(Vector3 position)
                    返回一条射线从摄像机通过一个屏幕点。
                    产生的射线是在世界空间中，从相机的近裁剪面开始并穿过屏幕position(x,y)像素坐标（position.z被忽略）。
                Physics.Raycast(Ray ray, out RaycastHit hitInfo)
                    当光线投射与任何碰撞器交叉时为真，否则为假。
                    如果返回true，hitInfo将包含碰到器碰撞的更多信息。
            */
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider && hitInfo.collider.tag == groundTag)
            {
                LookAtTarget(hitInfo.point);
                isReach = false;
            }
        }

        float distance = Vector3.Distance(transform.position, targetPosition);
        if(distance < minDistance)
        {
            isReach = true;
        }

        if (!isReach)
        {
            SimpleMove();
        }
    }

    // 玩家方向
    void LookAtTarget(Vector3 hitPoint)
    {
        targetPosition = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);  // 竖直方向保持不变
        transform.LookAt(targetPosition);
    }

    // 玩家移动
    void SimpleMove()
    {
        mController.SimpleMove(transform.forward * speed);  // 向前移动
    }
}
