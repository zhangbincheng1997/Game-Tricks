using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    [Header("对象池名字")]
    public string poolName;  
    [Header("对象池最大数量")]
    public int maxCount;
    [Header("对象池物体")]
    public GameObject poolGo;

    private List<GameObject> goList = new List<GameObject>();

    public Pool(string poolName, int maxCount, GameObject poolGo)
    {
        this.poolName = poolName;
        this.maxCount = maxCount;
        this.poolGo = poolGo;
    }

    // 从对象池获取物体
    public GameObject Spawn()
    {
        foreach (GameObject go in goList)
        {
            // 物体非空并且隐藏状态
            if (go != null && !go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;
            }
        }

        // 超过对象池最大数量
        if (goList.Count > maxCount)
        {
            GameObject temp = goList[0];
            // 从池中移除
            goList.RemoveAt(0);
            // 销毁物体
            GameObject.Destroy(temp);
        }

        // 实例化物体
        GameObject goTemp = GameObject.Instantiate(poolGo);
        // 对象池同名
        goTemp.name = poolName;
        // 放到对象池
        goList.Add(goTemp);
        return goTemp;
    }

    // 将物体放回对象池
    public void Unspawn(GameObject go)
    {
        go.SetActive(false);
    }
}
