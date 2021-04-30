using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Primus.Sample.ModTool.BeaconEditor.Beacon;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class PanelBeaconBlue : BasePanelBeacon
    {
        private BeaconBlue _beaconBlue;
        public new BeaconBlue Beacon { get => _beaconBlue; set { _beaconBlue = value; base.Beacon = value; } }

        protected override void Awake()
        {
            base.Awake();
        }

        protected virtual void Update()
        {
        }

        internal void ManualUpdate()
        {
            Beacon.RotationAngle -= 20.0f * Time.deltaTime;
        }
    }
}
