using System;

namespace Primus.Core.Bibliotheca
{
    public interface IBiblion<TBiblionTitle> where TBiblionTitle : Enum
    {
        void EnterRestitutionState();
        void EnterCirculationState();
    }
}