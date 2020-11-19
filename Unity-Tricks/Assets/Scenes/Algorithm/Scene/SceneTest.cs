using UnityEngine;

public class SceneTest : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneMgr.Instance.LoadScene(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SceneMgr.Instance.LoadScene(1);
        }
    }
}
