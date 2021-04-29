using System;
using UnityEngine;

namespace Primus.Core.Bibliotheca
{
    public abstract class BaseBiblion<TCatalogTitle> : MonoBehaviour, IBiblion<TCatalogTitle>
        where TCatalogTitle : Enum
    {
        //[field: SerializeField] 
        public TCatalogTitle BiblionTitle { get; protected set; }

        public virtual void EnterRestitutionState()
        {
            gameObject.SetActive(false);
        }

        public virtual void EnterCirculationState()
        {
            gameObject.SetActive(true);
        }
    }
}