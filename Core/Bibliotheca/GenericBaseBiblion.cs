using System;
using UnityEngine;

namespace Primus.Core.Bibliotheca
{
    public abstract class GenericBaseBiblion<TCatalogTitle> : MonoBehaviour, IBiblion<TCatalogTitle>
        where TCatalogTitle : Enum
    {
        [field: SerializeField] internal TCatalogTitle Title { get; set; }

        public virtual void EnterRestitutionState()
        {
        }

        public virtual void EnterCirculationState()
        {
        }
    }
}