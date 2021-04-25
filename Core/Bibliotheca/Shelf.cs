using System;
using System.Collections.Generic;
using UnityEngine;

namespace Primus.Core.Bibliotheca
{
    [Serializable]
    public class Shelf<TBiblionTitle> : IShelf<TBiblionTitle> where TBiblionTitle : Enum
    {
        private BaseBiblion<TBiblionTitle> _circulationCache;
        private Stack<BaseBiblion<TBiblionTitle>> _stack;

        [NonSerialized] public GameObject BiblionPrefab;
        [field: SerializeField] public TBiblionTitle Title { get; private set; }
        [field: SerializeField] public BaseBiblion<TBiblionTitle> Biblion { get; private set; }

        [field: SerializeField]
        [field: Range(1, 64)]
        public int BatchSize { get; private set; }

        public void PutBiblion(BaseBiblion<TBiblionTitle> biblion)
        {
            if (Title.Equals(biblion.Title))
            {
                biblion.EnterRestitutionState();
                _stack.Push(biblion);
                return;
            }

            Debug.LogError($"Product is a different Kind: {biblion.name}, {biblion.Title} ");
        }

        public BaseBiblion<TBiblionTitle> GetBiblion()
        {
            _circulationCache = _stack.Pop();
            _circulationCache.EnterCirculationState();
            return _circulationCache;
        }

        public void InitializeState()
        {
            BiblionPrefab = Biblion.gameObject;
            _stack = new Stack<BaseBiblion<TBiblionTitle>>();
        }
    }
}