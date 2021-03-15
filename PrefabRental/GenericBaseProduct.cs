using System;
using UnityEngine;

namespace Primus.PrefabRental
{
    public abstract class GenericBaseProduct<TEnum> : MonoBehaviour, IProduct where TEnum : Enum
    {
        [SerializeField] private TEnum _brand;
        public TEnum Brand
        {
            get => _brand;
            protected set => _brand = value;
        }
    }
}