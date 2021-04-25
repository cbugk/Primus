using System;
using UnityEngine;
using UnityEngine.UI;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class CanvasManager : MonoBehaviour
    {
        [NonSerialized] public PanelListBeacon PanelListBeacon;
        [NonSerialized] public PanelMain PanelMain;
        [NonSerialized] public PanelSelectionBeacon PanelSelectionBeacon;
        [NonSerialized] public PanelEscMenu PanelEscMenu;

        private void Awake()
        {
            PanelMain = GetComponentInChildren<PanelMain>();
            PanelSelectionBeacon = GetComponentInChildren<PanelSelectionBeacon>();
            PanelEscMenu = GetComponentInChildren<PanelEscMenu>();
            PanelListBeacon = GetComponentInChildren<PanelListBeacon>();
        }
        private void Start()
        {
            if (PanelEscMenu != null)
            {
                PanelEscMenu.gameObject.SetActive(false);
            }
            if (PanelListBeacon != null)
            {
                PanelListBeacon.gameObject.SetActive(false);
            }
        }
    }
}
