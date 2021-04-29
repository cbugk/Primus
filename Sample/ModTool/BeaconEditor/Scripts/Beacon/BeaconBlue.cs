using UnityEngine;
using Primus.Core.Bibliotheca;

namespace Primus.Sample.ModTool.BeaconEditor.Beacon
{
    public class BeaconBlue : BaseBeacon
    {
        protected override void Awake()
        {
            base.Awake();
            BiblionTitle = BeaconType.BLUE;
        }

        protected void Update()
        {
        }
    }
}