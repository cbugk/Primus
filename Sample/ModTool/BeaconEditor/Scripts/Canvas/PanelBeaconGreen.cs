using Primus.Sample.ModTool.BeaconEditor.Beacon;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class PanelBeaconGreen : BasePanelBeacon
    {
        private BeaconGreen _beaconGreen;
        public new BeaconGreen Beacon { get => _beaconGreen; set { _beaconGreen = value; base.Beacon = value; } }
    }
}

