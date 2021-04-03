using System;

namespace Primus.Core.Bibliotheca
{
    public interface IShelf<TBiblionTitle> where TBiblionTitle : Enum
    {
        GenericBaseBiblion<TBiblionTitle> GetBiblion();
        void PutBiblion(GenericBaseBiblion<TBiblionTitle> biblion);
    }
}