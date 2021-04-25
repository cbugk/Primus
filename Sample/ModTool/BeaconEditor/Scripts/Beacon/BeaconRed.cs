using System.Collections.Generic;

namespace Primus.Sample.ModTool.BeaconEditor.Beacon
{
    public class BeaconRed : BaseBeacon
    {
        private List<BeaconGreen> _childrenGreen;
        protected override void Awake()
        {
            base.Awake();
            Title = BeaconType.RED;
            _childrenGreen = new List<BeaconGreen>();
        }
    }
}