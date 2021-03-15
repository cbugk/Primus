using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Primus.Core;
using UnityEngine;

namespace Primus.PrefabRental
{
    public abstract class GenericRetailer<TEnum> : GenericSingleton<GenericRetailer<TEnum>> where TEnum : Enum
    {
        [SerializeField] protected GenericBaseProduct<TEnum>[] ProductCatalog;
        private int _catalogCount;
        private TEnum[] _brandCatalog;
        //protected Stock[] _stockCatalog;

        protected virtual void Start()
        {
            // Initialize fields/properties
            _catalogCount = ProductCatalog.Length;
            _brandCatalog = null;

            // Unless _productCatalog is ordered illegally, _brandCatalog is set
            ValidateCatalog();
        }

        public GameObject GetProduct(TEnum brand)
        {
            if (_brandCatalog != null)
            {
                int typeIndex = Array.BinarySearch(_brandCatalog, brand);
                if (0 <= typeIndex)
                {
                    return GameObject.Instantiate(ProductCatalog[typeIndex].gameObject);
                }
                else
                {
                    Debug.Log("Retailer Catalog does not include product of supplied type.");
                }
            }
            else
            {
                Debug.Log("Retailer Catalog is empty.");
            }

            return null;
        }

        private void ValidateCatalog()
        {
            if (_catalogCount == 0)
            {
                Debug.Log("Retailer Catalog is empty.");
            }
            else
            {
                _brandCatalog = new TEnum[_catalogCount];

                if (ProductCatalog[0] == null)
                {
                    throw new Exception("Null product in Retailer Catalog. Index: 0");
                }
                else
                {
                    _brandCatalog[0] = ProductCatalog[0].Brand;
                }

                for (int index = 1; index < _catalogCount; index++)
                {
                    if (ProductCatalog[index] == null)
                    {
                        throw new Exception($"Null product in Retailer Catalog. Index: {index}");
                    }
                    else
                    {
                        TEnum brandCache = ProductCatalog[index].Brand;

                        // Ensure types are unique while keeping search is cheap later on (strictly ascending order)
                        if (_brandCatalog[index - 1].CompareTo(brandCache) < 0)
                        {
                            _brandCatalog[index] = brandCache;
                        }
                        else
                        {
                            throw new Exception($"Retailer Catalog must be populated in ascending order. Index: {index}");
                        }
                    }
                }
            }
        }
    }
}