using System.Collections;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    public const string OBJ1_POOL = "black";
    public const string OBJ2_POOL = "red";

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject obj = PoolMgr.Instance.Spawn(OBJ1_POOL);
            obj.transform.SetParent(this.transform);
            obj.transform.position = new Vector3(0, 5, 0);
            StartCoroutine(Unspawn(obj, 5.0f));
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameObject obj = PoolMgr.Instance.Spawn(OBJ2_POOL);
            obj.transform.SetParent(this.transform);
            obj.transform.position = new Vector3(0, 5, 0);
            StartCoroutine(Unspawn(obj, 5.0f));
        }
    }

    IEnumerator Unspawn(GameObject obj, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        PoolMgr.Instance.Unspawn(obj);
    }
}
