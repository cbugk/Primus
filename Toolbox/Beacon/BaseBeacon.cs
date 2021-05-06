using System;
using UnityEngine;
using Primus.Core.Bibliotheca;

namespace Primus.Toolbox.Beacon
{
    public abstract class BaseBeacon<TCatalogBeacon>
        : BaseBiblion<TCatalogBeacon>
        where TCatalogBeacon : Enum
    {
        private float _rotationAngle;
        public float RotationAngle
        {
            get
            {
                return -_rotationAngle;
            }
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