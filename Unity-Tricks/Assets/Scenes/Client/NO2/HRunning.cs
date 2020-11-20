using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // 导入 DOTween 命名空间，DOTween强大！！！

public class HRunning : MonoBehaviour
{
    // 水平跑马灯
    public Text HText;
    public float bgWidth = 400.0f;
    public float duration = 10.0f;
    public float delay = 1.0f;

    void Start()
    {
        HPlay();
    }

    // 播放文字水平走马灯效果
    private void HPlay()
    {
        float width = HText.preferredWidth;  // 获取文字的长度
        HText.rectTransform.anchoredPosition = new Vector2(0, 0.5f);  // 让文字从在最右边开始移动  

        Tweener tweener = HText.rectTransform.DOLocalMoveX(-(bgWidth + width), duration);  // 设置动画持续时间
        tweener.SetDelay(delay);  // 设置动画延迟时间
        tweener.SetEase(Ease.Linear);  // 设置动画播放方式
        tweener.SetLoops(5, LoopType.Restart);  // 每次播放结束后重新开始播放，一共播放 5 次
        tweener.OnStart(delegate { Debug.Log("水平走马灯事件开始"); });  // 设置动画开始事件
        tweener.OnComplete(delegate { Debug.Log("水平走马灯事件结束"); });  // 设置动画结束事件
    }
}
