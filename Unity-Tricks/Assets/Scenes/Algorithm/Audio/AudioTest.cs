using UnityEngine;

public class AudioTest : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioMgr.Instance.PlayEffect("click");
            AudioMgr.Instance.PlayMusic("music");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            AudioMgr.Instance.PlayEffect("click");
            AudioMgr.Instance.PlayMusic("music2");
        }
    }
}
