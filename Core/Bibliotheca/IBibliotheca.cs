using System;
using UnityEngine;

namespace Primus.Core.Bibliotheca
{
    public interface IBibliotheca<TBiblionTitle> where TBiblionTitle : Enum
    {
        GameObject CheckOut(TBiblionTitle title);
        void CheckIn(GenericBaseBiblion<TBiblionTitle> biblion);
    }
}