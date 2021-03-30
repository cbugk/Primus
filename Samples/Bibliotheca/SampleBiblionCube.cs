using UnityEngine;
using Primus.Core.Bibliotheca;

namespace Primus.Sample.Bibliotheca
{
    public class SampleBiblionCube : GenericBaseBiblion<SampleTitleCatalog>
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