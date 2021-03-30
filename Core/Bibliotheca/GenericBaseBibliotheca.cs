﻿using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using Primus.Core;
using UnityEngine;
using Workbench;

namespace Primus.Core.Bibliotheca
{
    public abstract class GenericBaseBibliotheca<TBiblionTitle> : MonoBehaviour, IBibliotheca<TBiblionTitle> where TBiblionTitle : Enum
        //public abstract class GenericRetailer<TEnum> : GenericSingleton<GenericRetailer<TEnum>> where TEnum : Enum
    {
        [SerializeField] protected GenericShelf<TBiblionTitle>[] Inventory;
        private int _inventoryCount;
        private TBiblionTitle[] _titlesInInventory;

        protected virtual void Start()
        {
            // Initialize fields/properties
            _inventoryCount = Inventory.Length;
            _titlesInInventory = null;

            // Unless inventory is ordered illegally, _typesInInventory is set
            ValidateInventory();

        }

        public GameObject CheckOut(TBiblionTitle title)
        {
            int shelfIndex = TitleToShelfIndex(title);
            if (0 <= shelfIndex)
            {
                try
                {
                    return Inventory[shelfIndex].GetBiblion().gameObject;
                }
                catch (InvalidOperationException)
                {
                    GenericShelf<TBiblionTitle> respectiveShelf = Inventory[shelfIndex];
                    int batchSize = respectiveShelf.BatchSize;
                    for (int batchIndex = 0; batchIndex < batchSize; batchIndex++)
                    {
                        respectiveShelf.PutBiblion(ManufactureBiblion(shelfIndex));
                    }
                    return respectiveShelf.GetBiblion().gameObject;
                }
            }

            return null;
        }

        public void CheckIn(GenericBaseBiblion<TBiblionTitle> biblion)
        {
            Inventory[TitleToShelfIndex(biblion.Title)].PutBiblion(biblion);
        }

        private GenericBaseBiblion<TBiblionTitle> ManufactureBiblion(int shelfIndex)
        {
            // Should not return null for a proper setup.
            return GameObject.Instantiate(Inventory[shelfIndex].BiblionPrefab).GetComponent<GenericBaseBiblion<TBiblionTitle>>();
        }

        private int TitleToShelfIndex(TBiblionTitle title)
        {
            if (_titlesInInventory != null)
            {
                int shelfIndex = Array.BinarySearch(_titlesInInventory, title);
                if (0 <= shelfIndex)
                {
                    return shelfIndex;
                }
                else
                {
                    Debug.Log($"[{name}] Bibliotheca inventory does not include Biblion of supplied Title.");
                }
            }
            else
            {
                Debug.Log($"[{name}] Bibliotheca inventory is empty.");
            }

            return -1;
        }

        private void ValidateInventory()
        {
            if (_inventoryCount == 0)
            {
                Debug.Log($"[{name}] Bibliotheca inventory is empty.");
            }
            else
            {
                TBiblionTitle titleCache;

                _titlesInInventory = new TBiblionTitle[_inventoryCount];

                Inventory[0].InitializeState();

                if (Inventory[0].BiblionPrefab == null)
                {
                    throw new Exception($"[{name}] Null BiblionPrefab in Bibliotheca inventory. Index: 0");
                }
                else
                {
                    _titlesInInventory[0] = Inventory[0].Title;
                    for (var batchIndex = 0; batchIndex < Inventory[0].BatchSize; batchIndex++)
                    {
                        Inventory[0].PutBiblion(ManufactureBiblion(0).GetComponent<GenericBaseBiblion<TBiblionTitle>>());
                    }
                }

                for (int shelfIndex = 1; shelfIndex < _inventoryCount; shelfIndex++)
                {
                    Inventory[shelfIndex].InitializeState();

                    if (Inventory[shelfIndex].BiblionPrefab == null)
                    {
                        throw new Exception($"[{name}] Null BiblionPrefab in Bibliotheca inventory. Index: {shelfIndex}");
                    }
                    else
                    {
                        titleCache = Inventory[shelfIndex].Title;

                        // Ensure kinds are unique while keeping search is cheap later on (strictly ascending order)
                        if (_titlesInInventory[shelfIndex - 1].CompareTo(titleCache) < 0)
                        {
                            _titlesInInventory[shelfIndex] = titleCache;
                            for (var batchIndex = 0; batchIndex < Inventory[shelfIndex].BatchSize; batchIndex++)
                            {
                                Inventory[shelfIndex].PutBiblion(ManufactureBiblion(shelfIndex).GetComponent<GenericBaseBiblion<TBiblionTitle>>());
                            }
                        }
                        else
                        {
                            throw new Exception($"[{name}] Bibliotheca inventory must be populated in ascending order. Index: {shelfIndex}");
                        }
                    }
                }
            }
        }
    }
}