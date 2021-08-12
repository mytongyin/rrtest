//单例模板类
public class SingletonBase<T>
    where T : new()
{
    private static T instance;

    public static T GetInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    public void DestroyInstance()
    {
        instance = default(T);
    }
}
