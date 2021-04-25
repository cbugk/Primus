using System;
using UnityEngine;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class PanelMain : MonoBehaviour
    {
        [NonSerialized] public DropdownBeacon DropdownBeacon;
        private void Awake()
        {
            DropdownBeacon = gameObject.GetComponentInChildren<DropdownBeacon>();
        }

        private void Start()
        {
            if (DropdownBeacon)
            {
                DropdownBeacon.AddListener(value => BeaconEditorManager.Instance.BeaconTypeDropdownSelected = DropdownBeacon.GetTitle(value));
            }
            else
            {
                Debug.Log("Howww");
            }
        }
    }
}
