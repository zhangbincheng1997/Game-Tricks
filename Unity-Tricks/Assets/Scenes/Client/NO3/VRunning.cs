using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // 导入 DOTween 命名空间，DOTween强大！！！

public class VRunning : MonoBehaviour
{
    // 垂直跑马灯
    public Text VText;
    public float bgHeight = 250.0f;
    public float duration = 5.0f;
    public float delay = 1.0f;

    void Start()
    {
        VPlay();
    }

    // 播放文字水平走马灯效果
    private void VPlay()
    {
        float height = VText.preferredHeight;  // 获取文字的长度
        VText.rectTransform.anchoredPosition = new Vector2(0.5f, 1);  // 让文字从在最下边开始移动  

        Tweener tweener = VText.rectTransform.DOLocalMoveY(bgHeight + height, duration);  // 设置动画持续时间
        tweener.SetDelay(delay);  // 设置动画延迟时间
        tweener.SetEase(Ease.Linear);  // 设置动画播放方式
        tweener.SetLoops(5, LoopType.Restart);  // 每次播放结束后重新开始播放，一共播放 5 次
        tweener.OnStart(delegate { Debug.Log("垂直走马灯事件开始"); });  // 设置动画开始事件
        tweener.OnComplete(delegate { Debug.Log("垂直走马灯事件结束"); });  // 设置动画结束事件
    }
}
