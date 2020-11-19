// 普通单例
public abstract class Singleton<T>
    where T : new()
{
    private static T _instance;
    private static object _lock = new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 上锁，防止重复实例化
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}