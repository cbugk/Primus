using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Primus.PrefabRental;

namespace Primus.PrefabRental.Sample
{
    public class CubeProduct : BaseProduct
    {
        [SerializeField] private Product _type;
        
        public bool IsSpinning { get; set; }

        private void Awake()
        {
            IsSpinning = false;
            ProductId = (int) _type;
        }

        private void Update()
        {
            if (IsSpinning) transform.Rotate(Vector3.up, 1.0f);
        }
    }
}