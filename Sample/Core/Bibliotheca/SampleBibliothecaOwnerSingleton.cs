using Primus.Core;
using UnityEngine;

namespace Primus.Sample.Bibliotheca
{
    public class SampleBibliothecaOwnerSingleton : BaseMonoSingleton<SampleBibliothecaOwnerSingleton>
    {
        [field: SerializeField] public SampleBibliotheca Bibliotheca { get; private set; }
    }
}