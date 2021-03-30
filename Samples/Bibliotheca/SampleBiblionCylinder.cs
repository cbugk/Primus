using System.Collections;
using System.Collections.Generic;
using Primus.Core;
using Primus.Core.Bibliotheca;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Primus.Sample.Bibliotheca
{
    public class SampleBiblionCylinder : GenericBaseBiblion<SampleTitleCatalog>
    {
        [field: SerializeField] public bool IsSpinning { get; set; }

        private void Awake()
        {
        }

        private void Update()
        {
            if (IsSpinning) transform.Rotate(Vector3.right, 1.0f);
        }
    }
}
