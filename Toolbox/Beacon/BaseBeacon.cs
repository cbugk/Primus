using System;
using UnityEngine;
using Primus.Core.Bibliotheca;

namespace Primus.Toolbox.Beacon
{
    public abstract class BaseBeacon<TCatalogBeacon>
        : BaseBiblion<TCatalogBeacon>
        where TCatalogBeacon : Enum
    {
        [field: SerializeField] public float Radius { get; set; }

        /// <summary>Negative of RotationAngle, clock-wise.</summary>
        private float _rotationAngle;

        /// <summary>Cartesian angle on x-z plane in degrees (counter-clockwise, zero at x-positive).</summary>
        public float RotationAngle
        {
            get => -_rotationAngle;
            set
            {
                // Degrees in modulus 360.
                if (!value.Equals(360.0f))
                {
                    _rotationAngle = -(value % 360.0f);
                }
                else
                {
                    _rotationAngle = -value;
                }
                transform.eulerAngles = Vector3.up * _rotationAngle;
            }
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}