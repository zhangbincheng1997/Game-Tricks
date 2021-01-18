using UnityEngine;

public class RandPoint : MonoBehaviour {
    public GameObject prefabs;

    private float x_center = 0;
    private float y_center = 0;
    private float r = 10;

    void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            Vector3 pos = GetPoint();
            GameObject gameObject = Instantiate(prefabs, pos, Quaternion.identity);
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
    }

    public Vector3 GetPoint()
    {
        float p = Mathf.Sqrt(Random.Range(0, 1f)) * r;
        float theta = 2 * Mathf.PI * Random.Range(0, 1f);
        float x = x_center + p * Mathf.Cos(theta);
        float y = y_center + p * Mathf.Sin(theta);
        return new Vector3(x, y, 0);
    }
}
