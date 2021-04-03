using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core;
using Primus.ModTool.Functionality;

namespace Primus.ModTool.Core
{
    public class ModToolManager : GenericMonoSingleton<ModToolManager>
    {
        [SerializeField] private ModToolInput _modToolInput;
        [SerializeField] private PositionSelector _positionSelector;

        private bool _isModToolFunctionalityActive;

        protected override void Awake()
        {
            base.Awake();
            _isModToolFunctionalityActive = false;
            _modToolInput = new ModToolInput();

            //Initialize functionality
            _positionSelector.Initialize();
        }

        private void OnEnable()
        {
            _modToolInput.ModToolManager.ToggleActive.Enable();
            _modToolInput.ModToolManager.ToggleActive.performed += ToggleActive;

            _modToolInput.PositionSelector.SwitchToModCam.Enable();
            _modToolInput.PositionSelector.SwitchToModCam.performed += PositionSelectorSwitchCamera;
            //inputSwitchCamera.action.Enable();
        }

        private void OnDisable()
        {
            _modToolInput.PositionSelector.SwitchToModCam.performed -= PositionSelectorSwitchCamera;
            _modToolInput.PositionSelector.SwitchToModCam.Disable();

            //inputSwitchCamera.action.Disable();  
        }

        private void ToggleActive(InputAction.CallbackContext context)
        {
            _isModToolFunctionalityActive = !_isModToolFunctionalityActive;
            if (_isModToolFunctionalityActive)
            {
                Debug.Log($"[{name}]: Activated ModTool Functionality");
            }
            else
            {
                Debug.Log($"[{name}]: Disactivated ModTool Functionality");
            }
        }

        private void PositionSelectorSwitchCamera(InputAction.CallbackContext context)
        {
            if (_isModToolFunctionalityActive)
            {
                _positionSelector.SwitchCamera();
            }
        }
    }
}