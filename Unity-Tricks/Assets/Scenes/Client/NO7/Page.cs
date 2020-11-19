using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class Page : MonoBehaviour
{
    public Text panelText;  // 界面页数
    public Button preBtn;  // 上一页
    public Button nextBtn;  // 下一页

    private int itemsCount = 0;  // 物品数量
    private int pagesCount = 0;  // 页面数量
    private int pageIndex = 1;  // 当前页面
    private const int COUNT = 12;  // 一页物品数量

    private Vector3 from = new Vector3(1f, 1f, 1f);  // 动态变大
    private Vector3 to = new Vector3(0.8f, 0.8f, 0.8f);  // 动态变小

    private List<GridItem> itemList = new List<GridItem>();  // 物品列表

    void Start()
    {
        InitUI();
        InitItem();
    }

    void InitUI()
    {
        preBtn.onClick.AddListener(delegate { OnPreBtnClick(); });
        nextBtn.onClick.AddListener(delegate { OnNextBtnClick(); });
    }

    void InitItem()
    {
        // 准备一个存储着12生肖信息的数组
        GridItem[] items = new GridItem[]
        {
            new GridItem("鼠鼠","Mouse"),
            new GridItem("牛牛","Ox"),
            new GridItem("虎虎","Tiger"),
            new GridItem("兔兔","Rabbit"),
            new GridItem("龙龙","Dragon"),
            new GridItem("蛇蛇","Snake"),
            new GridItem("马马","Horse"),
            new GridItem("羊羊","Goat"),
            new GridItem("猴猴","Monkey"),
            new GridItem("鸡鸡","Rooster"),
            new GridItem("狗狗","Dog"),
            new GridItem("猪猪","Pig")
        };
        // 利用12生肖数组来随机生成列表
        int cnt = Random.Range(1, 100);
        for (int i = 0; i < cnt; ++i)
        {
            itemList.Add(items[Random.Range(0, items.Length)]);
            Debug.Log(itemList[i].itemName);
        }
        // 计算元素总个数
        itemsCount = itemList.Count;
        // 计算总页数
        pagesCount = (itemsCount % COUNT) == 0 ? itemsCount / COUNT : (itemsCount / COUNT) + 1;
        BindPage(pageIndex);
        // 更新界面页数
        panelText.text = string.Format("{0} / {1}", pageIndex.ToString(), pagesCount.ToString());
    }

    // 上一页
    void OnPreBtnClick()
    {
        if (pagesCount <= 0)
            return;
        // 第一页时禁止向前翻页
        if (pageIndex <= 1)
            return;
        pageIndex -= 1;
        if (pageIndex < 1)
            pageIndex = 1;

        BindPage(pageIndex);

        // 更新界面页数
        panelText.text = string.Format("{0} / {1}", pageIndex.ToString(), pagesCount.ToString());
    }

    // 下一页
    void OnNextBtnClick()
    {
        if (pagesCount <= 0)
            return;
        // 最后一页禁止向后翻页
        if (pageIndex >= pagesCount)
            return;

        pageIndex += 1;
        if (pageIndex >= pagesCount)
            pageIndex = pagesCount;

        BindPage(pageIndex);

        // 更新界面页数
        panelText.text = string.Format("{0} / {1}", pageIndex.ToString(), pagesCount.ToString());
    }

    // 绑定指定索引处的页面元素
    void BindPage(int index)
    {
        // 列表处理
        if (itemList == null || itemsCount <= 0)
            return;

        // 索引处理
        if (index < 0 || index > itemsCount)
            return;

        // 需要特别处理的是最后一页
        if (index == pagesCount)
        {
            // 最后一页剩下的元素数目为 itemsCount - COUNT * (index - 1)
            // 其中 COUNT * (index-1) 为前面元素数目
            int cnt = itemsCount - COUNT * (index - 1);
            for (int i = 0; i < cnt; ++i)
            {
                BindGridItem(transform.GetChild(i), itemList[COUNT * (index - 1) + i]);
                transform.GetChild(i).gameObject.SetActive(true);
            }
            for (int i = cnt; i < COUNT; ++i)
            {
                // 隐藏多余物品
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        // 其他情况正常显示
        else
        {
            for (int i = 0; i < COUNT; ++i)
            {
                BindGridItem(transform.GetChild(i), itemList[COUNT * (index - 1) + i]);
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    // 将一个GridItem实例绑定到指定的Transform上
    void BindGridItem(Transform trans, GridItem gridItem)
    {
        trans.GetComponent<Image>().sprite = LoadSprite(gridItem.iconPath);
        trans.Find("Item/Name").GetComponent<Text>().text = gridItem.itemName;
        trans.GetComponent<Button>().onClick.RemoveAllListeners();
        trans.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("当前点击的元素名称为 : " + gridItem.itemName);
        });
        // 产生动态效果
        trans.DOScale(from, 0.25f).SetLoops(3, LoopType.Yoyo).ChangeStartValue(to);
    }

    // 加载精灵
    Sprite LoadSprite(string assetName)
    {
        return Resources.Load(assetName, typeof(Sprite)) as Sprite;
        // return Resources.Load(assetName, new Sprite().GetType()) as Sprite;
    }
}
