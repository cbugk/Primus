using System;
using UnityEngine;
using UnityEngine.UI;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class PanelEscMenu : MonoBehaviour
    {
        [NonSerialized] public Button ButtonImport;
        [NonSerialized] public Button ButtonExport;
        [NonSerialized] public Button ButtonRefresh;

        private void Awake()
        {
            var buttons = GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                switch (button.name)
                {
                    case "Import":
                        ButtonImport = button;
                        break;
                    case "Export":
                        ButtonExport = button;
                        break;
                    case "Refresh":
                        ButtonRefresh = button;
                        break;
                }
            }
        }
    }
}