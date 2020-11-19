using System.Collections.Generic;
using UnityEngine;

public class PoolMgr : UnitySingleton<PoolMgr>
{
    [Header("对象池")]
    public Pool[] pools;
    
    private Dictionary<string, Pool> poolDict = new Dictionary<string, Pool>();
    
    public override void Awake() { base.Awake(); Init(); }

    void Init()
    {
        foreach(Pool pool in pools)
        {
            poolDict.Add(pool.poolName, new Pool(pool.poolName, pool.maxCount, pool.poolGo));
        }
    }

    // 从对象池获取物体
    public GameObject Spawn(string poolName)
    {
        Pool pool;
        poolDict.TryGetValue(poolName, out pool);
        if (pool != null)
        {
            return pool.Spawn();
        }
        else
        {
            Debug.LogError(poolName + " Error");
            return null;
        }
    }

    // 将物体放回对象池
    public void Unspawn(GameObject go)
    {
        Pool pool;
        poolDict.TryGetValue(go.name, out pool);
        if (pool != null)
        {
            pool.Unspawn(go);
        }
        else
        {
            Debug.LogError(go.name + " Error");
        }
    }
}
