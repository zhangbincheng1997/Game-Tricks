using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMgr : UnitySingleton<SceneMgr>
{
    // 画布的transform
    private Transform _canvasTransform;
    private Transform CanvasTransform
    {
        get
        {
            if (_canvasTransform == null)
            { _canvasTransform = GameObject.Find("Canvas").transform; }
            return _canvasTransform;
        }
    }

    public GameObject loadScenePrefab;
    private Slider loadingBar;
    private Text loadingProgress;

    public void LoadScene(int sceneId)
    {
        // 初始化
        GameObject go = Instantiate(loadScenePrefab);
        go.transform.SetParent(CanvasTransform);
        go.transform.localPosition = Vector3.zero;
        // 获取组件
        loadingBar = go.GetComponentInChildren<Slider>();
        loadingProgress = go.GetComponentInChildren<Text>();
        // 开启协程
        StartCoroutine("LoadNormalScene", sceneId);
    }

    // 协程加载场景
    IEnumerator LoadNormalScene(int sceneId)
    {
        int startProgress = 0;
        int displayProgress = 0;
        int toProgress;

        // 异步加载场景
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneId);

        // 不激活场景
        op.allowSceneActivation = false;

        /*
            progress的取值范围在[0.1, 1)之间。
            progress可能在0.9的时候就直接进入新场景。
            所以需要分别控制两种进度[0.1, 0.9)和[0.9, 1)。
        */

        // [0.1, 0.9)
        while (op.progress < 0.9f)
        {
            toProgress = startProgress + (int)(op.progress * 100);
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetProgress(displayProgress);
                yield return null;
            }
            yield return null;
        }

        // [0.9, 1)
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            SetProgress(displayProgress);
            yield return null;
        }

        // 激活场景
        op.allowSceneActivation = true;
    }

    // 设置进度
    void SetProgress(int progress)
    {
        loadingBar.value = progress * 0.01f;
        loadingProgress.text = progress.ToString() + " %";
    }
}
