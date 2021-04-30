using System;
using UnityEngine;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class PanelListBeacon : MonoBehaviour
    {
        [NonSerialized] public ScrollListBeacon ScrollListBeacon;

        private void Awake()
        {
            ScrollListBeacon = GetComponentInChildren<ScrollListBeacon>();
        }
    }
}