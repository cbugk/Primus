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
                OnBeaconChanged(value);
            }
        }

        private int _panelIndex;
        private GameObject _beaconInstance;

        private void Awake()
        {
            _beaconInstance = null;
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

        /// <summary>Updates children beacon panels' states.</summary>
        private void OnBeaconChanged(GameObject newBeaconInstance)
        {
            // If beacon instance did not change
            if (_beaconInstance == newBeaconInstance)
            {
                return;
            }

            // If null, disable previously active panel.
            if (!newBeaconInstance)
            {
                DisablePanelBeacon(_panelIndex);
                _panelIndex = -1;
            }
            // Else, disable previously active panel and enable new one.
            else
            {
                // Disable previous panel.            
                DisablePanelBeacon(_panelIndex);
                // Enable new panel and set new panel index.
                EnablePanelBeacon(newBeaconInstance, true);
            }

            // Save new instance
            _beaconInstance = newBeaconInstance;
        }

        private void DisablePanelBeacon(int index)
        {
            if (0 <= index)
            {
                PanelBeacons[index].ClearBeacon();
                PanelBeacons[index].gameObject.SetActive(false);
            }
        }

        private void EnablePanelBeacon(GameObject beaconInstance, bool saveIndex)
        {
            BeaconType beaconTitle = beaconInstance.GetComponent<BaseBeacon>().BiblionTitle;

            int index = TitleToIndex(beaconTitle);

            if (0 <= index)
            {
                PanelBeacons[index].gameObject.SetActive(true);

                switch (beaconTitle)
                {
                    case BeaconType.BLUE:
                        ((PanelBeaconBlue)PanelBeacons[index]).Beacon = beaconInstance.GetComponent<BeaconBlue>();
                        break;
                    case BeaconType.GREEN:
                        ((PanelBeaconGreen)PanelBeacons[index]).Beacon = beaconInstance.GetComponent<BeaconGreen>();
                        break;
                    case BeaconType.RED:
                        ((PanelBeaconRed)PanelBeacons[index]).Beacon = beaconInstance.GetComponent<BeaconRed>();
                        break;
                }

                PanelBeacons[index].UpdatePanelRotation();
                PanelBeacons[index].UpdateFieldName();

                if (saveIndex)
                {
                    _panelIndex = index;
                }
            }
        }

        private int TitleToIndex(BeaconType biblionTitle)
        {
            int index = -1;
            switch (biblionTitle)
            {
                case BeaconType.BLUE:
                    index = 0;
                    break;
                case BeaconType.GREEN:
                    index = 1;
                    break;
                case BeaconType.RED:
                    index = 2;
                    break;
            }
            return index;
        }
    }
}
