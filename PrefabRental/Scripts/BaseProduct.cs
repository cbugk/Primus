using UnityEngine;

namespace Primus.PrefabRental
{
    public abstract class BaseProduct : MonoBehaviour, IProduct
    {
        public int ProductId { get; protected set;}
    }
}