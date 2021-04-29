using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace Primus.Sample.ModTool.BeaconEditor.IO
{
    [Serializable]
    public class BeaconEditorState
    {
        public readonly string Version = "v0.0.1_sample";
        public string Name = "";
        public BeaconState[] BeaconStates;

        [Serializable]
        public class BeaconState
        {
            public string Name;
            public string Type;
            public float3 Position;
            public float RotationAngle;
        }

        [Serializable]
        public struct float3
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
}