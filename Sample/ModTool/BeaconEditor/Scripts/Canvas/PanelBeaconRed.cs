using UnityEngine;
using UnityEngine.UI;
using Primus.Sample.ModTool.BeaconEditor.Beacon;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class PanelBeaconRed : BasePanelBeacon
    {
        private BeaconRed _beaconRed;
        public new BeaconRed Beacon { get => _beaconRed; set { _beaconRed = value; base.Beacon = value; } }
        public ScrollRect rect;

        protected override void Awake()
        {
            base.Awake();

            rect = GetComponentInChildren<ScrollRect>();
        }
    }
}
