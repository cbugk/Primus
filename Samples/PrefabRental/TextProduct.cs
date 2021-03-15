using System.Collections;
using System.Collections.Generic;
using Primus.PrefabRental;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Primus.Sample.PrefabRental
{
    public class TextProduct : GenericBaseProduct<ProductBrand>
    {
        public bool IsSpinning { get; set; }

        private void Awake()
        {
            Brand = ProductBrand.Text;
            IsSpinning = false;
        }

        private void Update()
        {
            transform.Rotate(Vector3.forward, 1.0f);
        }
    }
}