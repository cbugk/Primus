using System;
using UnityEngine;
using Primus.Sample.ModTool.BeaconEditor.Beacon;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class PanelSelectionBeacon : MonoBehaviour
    {
        [NonSerialized] public BasePanelBeacon[] PanelBeacons;
        public GameObject BeaconInstance
        {
            get => _beaconInstance;
            set
            {
                _beaconInstance = value;
                UpdateChildrenPanels();
            }
        }

        private BeaconType _beaconTitle;
        private int _panelIndex;
        private GameObject _beaconInstance;

        private void Awake()
        {
            _beaconTitle = BeaconType.INVALID;
            _panelIndex = -1;

            PanelBeacons = new BasePanelBeacon[3]
            {
                GetComponentInChildren<PanelBeaconBlue>(),
                GetComponentInChildren<PanelBeaconGreen>(),
                GetComponentInChildren<PanelBeaconRed>()
            };
        }

        private void Start()
        {
            foreach (Component panel in PanelBeacons)
            {
                panel.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
        }

        private void UpdateChildrenPanels()
        {
            // If null, disable previously active panel.
            if (!_beaconInstance)
            {
                _beaconTitle = BeaconType.INVALID;
                if (0 <= _panelIndex)
                {
                    PanelBeacons[_panelIndex].ClearBeacon();
                    PanelBeacons[_panelIndex].gameObject.SetActive(false);
                }
                _panelIndex = -1;
            }
            // Get BeaconType of selected beacon.
            else
            {
                _beaconTitle = _beaconInstance.GetComponent<BaseBeacon>().Title;

                // Convert into index in array.
                int index;
                switch (_beaconTitle)
                {
                    case BeaconType.BLUE:
                        index = 0;
                        ((PanelBeaconBlue)PanelBeacons[index]).Beacon = _beaconInstance.GetComponent<BeaconBlue>();
                        break;
                    case BeaconType.GREEN:
                        index = 1;
                        ((PanelBeaconGreen)PanelBeacons[index]).Beacon = _beaconInstance.GetComponent<BeaconGreen>();
                        break;
                    case BeaconType.RED:
                        index = 2;
                        ((PanelBeaconRed)PanelBeacons[index]).Beacon = _beaconInstance.GetComponent<BeaconRed>();
                        break;
                    default:
                        index = -1;
                        break;
                }

                // When selected beacon changes, disable previous and enable new panel.
                if (index != _panelIndex)
                {
                    if (0 <= _panelIndex)
                    {
                        PanelBeacons[_panelIndex].ClearBeacon();
                        PanelBeacons[_panelIndex].gameObject.SetActive(false);
                    }
                    if (0 <= index)
                    {
                        PanelBeacons[index].gameObject.SetActive(true);
                        PanelBeacons[index].UpdatePanelRotation();
                        PanelBeacons[index].UpdateFieldName();
                    }
                    _panelIndex = index;
                }
            }
        }
    }
}