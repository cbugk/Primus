using Primus.Core.Bibliotheca;
using UnityEngine;

namespace Primus.Sample.Bibliotheca
{
    public class SampleBiblionCube : BaseBiblion<SampleTitleCatalog>
    {
        [field: SerializeField] public bool IsSpinning { get; set; }

        private void Awake()
        {
        }

        private void Update()
        {
            if (IsSpinning) transform.Rotate(Vector3.up, 1.0f);
        }
    }
}