using UnityEngine;
using UnityEngine.InputSystem;
using Primus.ModTool.Core;

namespace Primus.ModTool.Functionality
{
    [System.Serializable]
    public class PositionSelector
    {
        // List of shortcuts per mod functionality
        // Mod: ModManager Camera switching functionality
        //[SerializeField] private InputActionReference inputSwitchCamera;
        [SerializeField] private CameraSwitcher cameraSwitcher;

        internal void Initialize()
        {
            if (cameraSwitcher != null)
            {
                cameraSwitcher.Initialize();
            }
        }

        internal void SwitchCamera()
        {
            cameraSwitcher.Switch();
        }
    }
}