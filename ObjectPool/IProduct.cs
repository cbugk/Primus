/// <summary>
/// Interface for poolable objects
/// </summary>
public interface IProduct
{
    IPool Origin { get; set; }

    void PrepareToUse();
    void ReturnToPool();
}