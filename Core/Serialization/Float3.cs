using System;
using UnityEngine;

namespace Primus.Core.Serialization
{
    [Serializable]
    public struct Float3
    {
        public float x;
        public float y;
        public float z;
        public Vector3 Vector3
        {
            get => new Vector3(x, y, z);
            set { x = value.x; y = value.y; z = value.z; }
        }
    }
}
