using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatRoom : MonoBehaviour
{
    public Button sendBtn;
    public InputField contentField;
    public Scrollbar scrollbarVertical;
    public Transform itemParent;
    public GameObject leftPrefab;
    public GameObject rightPrefab;

    private float minWidth = 100.0f;  // 聊天框最小宽度
    private float maxWidth = 400.0f;  // 聊天框最大宽度
    private float iconHeight = 100.0f;  // 头像高度
    private float chatHeight = 10.0f;  // 聊天框间隔
    private float allHeight = 0.0f;  // 聊天框总高度

    private float marginWidth = 40.0f;  // 宽度边距
    private float marginHeight = 40.0f;  // 高度边距

    private int historyCnt = 10;  // 历史条数

    private List<GameObject> itemList = new List<GameObject>();  // 历史聊天框列表

    void Awake()
    {
        // 注册事件
        scrollbarVertical.onValueChanged.AddListener(ScrollBarValueChanged);
        sendBtn.onClick.AddListener(delegate () { OnSendBtnClick(); });
    }

    void Update()
    {
        // 模拟女生聊天
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnGirlChat();
        }
        // 模拟男生聊天
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            OnBoyChat();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            OnSendBtnClick();
        }
    }

    bool isAddMessage = false;
    // 保证每次有消息自动滑动到最底部，同时保证没有消息到达时允许向上滑动
    public void ScrollBarValueChanged(float value)
    {
        if (isAddMessage)
        {
            scrollbarVertical.value = 0;
            isAddMessage = false;
        }
    }

    void OnSendBtnClick()
    {
        string content = contentField.text;
        if (content == "")
        {
            return;
        }

        string username = "太子";
        string iconPath = "player";
        CreateItem(content, username, iconPath, rightPrefab);
        contentField.text = "";
    }

    void OnGirlChat()
    {
        string content = "我最美美美！！！";
        string username = "女女";
        string iconPath = "girl";
        CreateItem(content, username, iconPath, leftPrefab);
    }

    void OnBoyChat()
    {
        string content = "我最帅帅帅！！！";
        string username = "男男";
        string iconPath = "boy";
        CreateItem(content, username, iconPath, leftPrefab);
    }

    void CreateItem(string content, string username, string iconPath, GameObject prefab)
    {
        // 初始化
        GameObject tempGo = Instantiate(prefab);
        tempGo.transform.SetParent(itemParent);
        tempGo.transform.localPosition = Vector3.zero;
        tempGo.transform.localScale = Vector3.one;

        // 更新显示
        tempGo.GetComponent<ChatUI>().UpdateItem(content, username, iconPath);

        // 适应屏幕
        FitScreen(tempGo);

        // 存储聊天框
        itemList.Add(tempGo);
        Clear();
    }

    // 控制适应聊天框
    void FitScreen(GameObject tempGo)
    {
        Text tempChatText = tempGo.transform.Find("Content").GetComponent<Text>();
        if (tempChatText.preferredWidth + marginWidth < minWidth)  // 单行宽度太短，宽度至少为 minWidth
        {
            tempGo.GetComponent<RectTransform>().sizeDelta = new Vector2(minWidth, tempChatText.preferredHeight + marginHeight);
            tempChatText.GetComponent<RectTransform>().sizeDelta = new Vector2(minWidth, tempChatText.preferredHeight + marginHeight);
        }
        else if (tempChatText.preferredWidth + marginWidth > maxWidth)  // 单行宽度太长，宽度至多为 maxWidth
        {
            tempGo.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, tempChatText.preferredHeight + marginHeight);
            tempChatText.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth - marginWidth, tempChatText.preferredHeight + marginHeight);
        }
        else  // 不长不短，文字自适应聊天框
        {
            tempGo.GetComponent<RectTransform>().sizeDelta = new Vector2(tempChatText.preferredWidth + marginWidth, tempChatText.preferredHeight + marginHeight);
            tempChatText.GetComponent<RectTransform>().sizeDelta = new Vector2(tempChatText.preferredWidth, tempChatText.preferredHeight + marginHeight);
        }

        tempChatText.SetVerticesDirty();  // 通知 Layout 布局需要重建
        tempGo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -allHeight);  // 相对于中心点设置位置
        allHeight += (tempChatText.preferredHeight + marginHeight) + iconHeight + chatHeight;  // 增加高度，包括文字背景、头像高度和聊天框间隔
        if (allHeight > itemParent.GetComponent<RectTransform>().sizeDelta.y)  // 超出父容器，父容器伸长
        {
            itemParent.GetComponent<RectTransform>().sizeDelta = new Vector2(itemParent.GetComponent<RectTransform>().sizeDelta.x, allHeight);
        }
    }

    // 移除历史聊天框
    void Clear()
    {
        if (itemList.Count > historyCnt)
        {
            Destroy(itemList[0]);
            itemList.RemoveAt(0);

            allHeight = 0.0f;
            itemParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 0.0f);
            foreach (var item in itemList)
            {
                FitScreen(item.gameObject);  // 重新排布 UI
            }
        }
        isAddMessage = true;
        scrollbarVertical.value = 0.1f;  // 利用 scrollbar 调整排版
    }
}
