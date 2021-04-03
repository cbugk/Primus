using System;
using System.Collections.Generic;
using UnityEngine;

namespace Primus.Core.Bibliotheca
{
    [Serializable]
    public class GenericShelf<TBiblionTitle> : IShelf<TBiblionTitle> where TBiblionTitle : Enum
    {
        private GenericBaseBiblion<TBiblionTitle> _circulationCache;
        private Stack<GenericBaseBiblion<TBiblionTitle>> _stack;

        [NonSerialized] public GameObject BiblionPrefab;
        [field: SerializeField] public TBiblionTitle Title { get; private set; }
        [field: SerializeField] public GenericBaseBiblion<TBiblionTitle> Biblion { get; private set; }

        [field: SerializeField]
        [field: Range(1, 64)]
        public int BatchSize { get; private set; }

        public void PutBiblion(GenericBaseBiblion<TBiblionTitle> biblion)
        {
            if (Title.Equals(biblion.Title))
            {
                biblion.EnterRestitutionState();
                _stack.Push(biblion);
                return;
            }

            Debug.LogError($"Product is a different Kind: {biblion.name}, {biblion.Title} ");
        }

        public GenericBaseBiblion<TBiblionTitle> GetBiblion()
        {
            _circulationCache = _stack.Pop();
            _circulationCache.EnterCirculationState();
            return _circulationCache;
        }

        public void InitializeState()
        {
            BiblionPrefab = Biblion.gameObject;
            _stack = new Stack<GenericBaseBiblion<TBiblionTitle>>();
        }
    }
}