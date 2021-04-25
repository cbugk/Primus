using System;

namespace Primus.Core.Bibliotheca
{
    public interface IShelf<TBiblionTitle> where TBiblionTitle : Enum
    {
        BaseBiblion<TBiblionTitle> GetBiblion();
        void PutBiblion(BaseBiblion<TBiblionTitle> biblion);
    }
}