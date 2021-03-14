﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Primus.PrefabRental.Sample
{
    public class TextProduct : BaseProduct
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
            transform.Rotate(Vector3.forward, 1.0f);
        }
    }
}