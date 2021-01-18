using UnityEngine;
using UnityEngine.UI;

public class Reference : MonoBehaviour
{
    //颜色材质区分
    public Material startMat;
    public Material endMat;
    public Material obstacleMat;
    //显示信息Text
    private Text text;
    //当前格子坐标
    public int x;
    public int y;

    void Awake ()
    {
        text = GameObject.Find ("Text").GetComponent<Text> ();
    }
    //判断当前格子的类型
    void OnTriggerEnter (Collider other)
    {
        if (other.name == "Start") {
            GetComponent<MeshRenderer> ().material = startMat;
            AStar.instance.grids [x, y].type = GridType.Start;
            AStar.instance.openList.Add (AStar.instance.grids [x, y]);
            AStar.instance.startX = x;
            AStar.instance.startY = y;
        } else if (other.name == "End") {
            GetComponent<MeshRenderer> ().material = endMat;
            AStar.instance.grids [x, y].type = GridType.End;
            AStar.instance.targetX = x;
            AStar.instance.targetY = y;
        } else if (other.name == "Obstacle") {
            GetComponent<MeshRenderer> ().material = obstacleMat;
            AStar.instance.grids [x, y].type = GridType.Obstacle;
        }
    }

    /// <summary>
    /// 鼠标点击显示当前格子基础信息
    /// </summary>
    void OnMouseDown ()
    {
        text.text = "XY(" + x + "," + y + ")" + "\n" +
        "FGH(" + AStar.instance.grids [x, y].f + "," +
        AStar.instance.grids [x, y].g + "," +
        AStar.instance.grids [x, y].h + ")";
        text.color = GetComponent<MeshRenderer> ().material.color;
    }
}