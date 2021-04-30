using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primus.Sample.ModTool.BeaconEditor.Beacon;

namespace Primus.Sample.ModTool.BeaconEditor.IO
{
    public static class ImportManager
    {
        public static void Load(BeaconEditorState beaconEditorState)
        {
            BeaconEditorManager.Instance.ClearBeacons();
            foreach (var beaconState in beaconEditorState.BeaconStates)
            {
                ParseBeacon(beaconState.Type, beaconState.Name, beaconState.Position.Vector3, beaconState.RotationAngle = 0);
            }
        }

        static void ParseBeacon(string beaconTypeString, string name, Vector3 position, float rotationAngle)
        {
            BeaconType beaconType = (BeaconType)System.Enum.Parse(typeof(BeaconType), beaconTypeString);
            GameObject beaconInstance = BeaconEditorManager.Instance.Bibliotheca.CheckOut(beaconType);

            if (beaconInstance)
            {
                beaconInstance.name = name;
                beaconInstance.transform.position = position;
                beaconInstance.GetComponent<BaseBeacon>().RotationAngle = rotationAngle;
                BeaconEditorManager.Instance.EnlistBeaconInstance(beaconInstance);
            }
        }
    }
}
