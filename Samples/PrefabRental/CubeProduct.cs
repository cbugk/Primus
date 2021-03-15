using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Primus.PrefabRental;

namespace Primus.Sample.PrefabRental
{
    public class CubeProduct : GenericBaseProduct<ProductBrand>
    {
        public bool IsSpinning { get; set; }

        private void Awake()
        {
            Brand = ProductBrand.Cube;
            IsSpinning = false;
        }

        private void Update()
        {
            if (IsSpinning) transform.Rotate(Vector3.up, 1.0f);
        }
    }
}