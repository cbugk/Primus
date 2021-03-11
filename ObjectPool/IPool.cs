public interface IPool
{
    void ReturnToPool(object instance);
}

public interface IPool<T> : IPool where T : IProduct
{
    T GetPrefabInstance();
    void ReturnToPool(T instance);
}