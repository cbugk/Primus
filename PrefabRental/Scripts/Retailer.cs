using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Primus.Core;
using UnityEngine;

namespace Primus.PrefabRental
{
    public class Retailer : GenericSingleton<Retailer>
    {
        [SerializeField] private BaseProduct[] _catalog;
        private int _catalogCount;
        private int[] _idCatalog;
        private Stock[] _stockCatalog;

        protected void Start()
        {
            // Initialize fields/properties
            _catalogCount = _catalog.Length;
            _idCatalog = null;

            // Unless _catalog is ordered illegally, _idCatalog is set
            ValidateCatalog();
        }

        public GameObject GetProduct(int id)
        {
            if (_idCatalog != null)
            {
                int typeIndex = Array.BinarySearch(_idCatalog, id);
                if (0 <= typeIndex)
                {
                    return GameObject.Instantiate(_catalog[typeIndex].gameObject);
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
                _idCatalog = new int[_catalogCount];

                if (_catalog[0] == null)
                {
                    throw new Exception("Null product in Retailer Catalog. Index: 0");
                }
                else
                {
                    _idCatalog[0] = _catalog[0].ProductId;
                }

                for (int index = 1; index < _catalogCount; index++)
                {
                    if (_catalog[index] == null)
                    {
                        throw new Exception($"Null product in Retailer Catalog. Index: {index}");
                    }
                    else
                    {
                        int idCache = _catalog[index].ProductId;

                        // Ensure types are unique while keeping search is cheap later on (strictly ascending order)
                        if (_idCatalog[index - 1] < idCache)
                        {
                            _idCatalog[index] = idCache;
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