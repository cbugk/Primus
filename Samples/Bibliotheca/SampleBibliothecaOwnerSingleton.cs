using UnityEngine;
using Primus.Core;

namespace Primus.Sample.Bibliotheca
{
    public class SampleBibliothecaOwnerSingleton : GenericSingleton<SampleBibliothecaOwnerSingleton>
    {
        [field: SerializeField] public SampleBibliotheca Bibliotheca { get; private set; }
    }
}