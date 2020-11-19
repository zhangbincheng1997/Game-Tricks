using System.Collections.Generic;
using UnityEngine;

public class Detecting : MonoBehaviour
{
    public Transform target; // 目标

    public float maxDistance = 5.0f; // 最大距离
    public float maxAngle = 90.0f; // 最大角度

    void Update()
    {
        // 玩家位置
        Vector3 pos = transform.position;
        // 目标位置
        Vector3 tarPos = target.position;
        // 计算距离
        float distance = Vector3.Distance(pos, tarPos);
        // 玩家正方向
        Vector3 normal = transform.rotation * Vector3.forward;
        // 玩家到目标的方向
        Vector3 offset = tarPos - pos;
        // 计算夹角
        float angle = Mathf.Acos(Vector3.Dot(normal.normalized, offset.normalized)) * Mathf.Rad2Deg;

        Draw(transform, maxDistance, maxAngle);

        // 判断是否在范围之内
        if (distance <= maxDistance && angle <= maxAngle / 2)
        {
            Debug.Log("目标在范围内...");
        }
        else
        {
            Debug.Log("目标不在范围内...");
        }
    }

    GameObject go;
    MeshFilter mf;
    MeshRenderer mr;
    Shader shader;

    // 绘制扇形
    public void Draw(Transform t, float radius, float angle)
    {
        int pointAmount = 100;
        float eachAngle = angle / pointAmount;

        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(t.position);
        for (int i = 0; i < pointAmount; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, -angle / 2 + eachAngle * (i - 1), 0f) * t.forward * radius + t.position;
            vertices.Add(pos);
        }
        CreateMesh(vertices);
    }

    // 创建网格
    private GameObject CreateMesh(List<Vector3> vertices)
    {
        int[] triangles;
        Mesh mesh = new Mesh();

        int triangleAmount = vertices.Count - 2;
        triangles = new int[3 * triangleAmount];

        // 根据三角形的个数，来计算绘制三角形的顶点顺序
        for (int i = 0; i < triangleAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;
        }

        if (go == null)
        {
            go = new GameObject("mesh");
            go.transform.position = new Vector3(0f, 0.1f, 0.5f);

            mf = go.AddComponent<MeshFilter>();
            mr = go.AddComponent<MeshRenderer>();

            shader = Shader.Find("Unlit/Color");
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

        mf.mesh = mesh;
        mr.material.shader = shader;
        mr.material.color = Color.red;

        return go;
    }
}
