using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primus.Sample.ModTool.BeaconEditor.Beacon;

namespace Primus.Sample.ModTool.BeaconEditor.IO
{
    public static class ExportManager
    {
        public static BeaconEditorState BeaconEditorState;
        public static void GenerateExport()
        {
            BeaconEditorState = new BeaconEditorState();
            List<GameObject> beaconInstances = BeaconEditorManager.Instance.BeaconInstances;

            BeaconEditorState.BeaconStates = new BeaconEditorState.BeaconState[beaconInstances.Count];
            for (int i = 0; i < beaconInstances.Count; i++)
            {
                BaseBeacon beacon = beaconInstances[i].GetComponent<BaseBeacon>();
                BeaconEditorState.BeaconStates[i] = new BeaconEditorState.BeaconState();
                BeaconEditorState.BeaconStates[i].Name = beaconInstances[i].name;
                BeaconEditorState.BeaconStates[i].Type = beacon.Title.ToString();
                BeaconEditorState.BeaconStates[i].RotationAngle = beacon.RotationAngle;
                BeaconEditorState.BeaconStates[i].Position.Vector3 = beaconInstances[i].transform.position;
            }
        }
    }
}