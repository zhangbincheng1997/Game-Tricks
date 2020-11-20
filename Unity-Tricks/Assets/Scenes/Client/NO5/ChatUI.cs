using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public Text contentText;
    public Text usernameText;
    public Image iconImage;

    public void UpdateItem(string content, string username, string iconPath)
    {
        contentText.text = content;
        usernameText.text = username;
        // iconImage.sprite = Resources.Load(iconPath, new Sprite().GetType()) as Sprite;
    }
}
