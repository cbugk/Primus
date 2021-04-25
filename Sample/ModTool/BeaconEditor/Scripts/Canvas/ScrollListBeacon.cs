using System;
using System.Collections.Generic;
using UnityEngine;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class ScrollListBeacon : MonoBehaviour
    {
        [NonSerialized] public List<ButtonBeacon> buttonBeacons;
        private ButtonBeacon _buttonCache;
        private List<GameObject> _beaconInstances;


        private void Awake()
        {
            _beaconInstances = BeaconEditorManager.Instance.BeaconInstances;
        }

        public void Add(ButtonBeacon button)
        {
            buttonBeacons.Add(button);
        }

        public void RemoveAt(int index)
        {
            buttonBeacons.RemoveAt(index);
        }

        public void ClearAndPopulate()
        {
            buttonBeacons.Clear();

            for (int index = 0; index < _beaconInstances.Count; index++)
            {
                _buttonCache = new ButtonBeacon();
                _buttonCache.Text = _beaconInstances[index].name;
                Add(_buttonCache);
            }
            _buttonCache = null;
        }
    }
}
