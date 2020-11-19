using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("移动速度")]
    public float speed = 5.0f;

    private Rigidbody mRigidbody;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0.05f || Mathf.Abs(v) > 0.05f)
        {
            Vector3 targetPos = new Vector3(h, 0, v);

            mRigidbody.velocity = targetPos * speed;
            transform.rotation = Quaternion.LookRotation(targetPos);
        }
        else
        {
            mRigidbody.velocity = Vector3.zero;
        }
    }
}
