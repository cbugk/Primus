using System;
using UnityEngine;
using Primus.Core.Bibliotheca;

namespace Primus.Sample.ModTool.BeaconEditor.Beacon
{
    public class BeaconGreen : BaseBeacon
    {
        protected override void Awake()
        {
            base.Awake();
            Title = BeaconType.GREEN;
        }
    }
}