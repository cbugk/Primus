using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core;
using Primus.ModTool.Core;

namespace Primus.Sample.ModTool.BeaconEditor
{
    public partial class BeaconEditorManager : BaseMonoSingleton<BeaconEditorManager>, IFunctionalityManager
    {

        private void AddCanvasListeners()
        {
            // Set listeners for Canvas elements
            _canvasManager.PanelEscMenu.ButtonImport.onClick.AddListener(LoadState);
            _canvasManager.PanelEscMenu.ButtonExport.onClick.AddListener(SaveState);
            _canvasManager.PanelEscMenu.ButtonRefresh.onClick.AddListener(ClearAndPopulateListButtonBeacon);
        }
        private void OnEnable()
        {
            _beaconEditorInput.BeaconEditorManager.ToggleActive.Enable();
            _beaconEditorInput.BeaconEditorManager.ToggleActive.performed += OnToggleActive;

            _beaconEditorInput.Beacon.Delete.Enable();
            _beaconEditorInput.Beacon.Delete.performed += OnBeaconDelete;

            _beaconEditorInput.CameraManager.SwitchToModCam.Enable();
            _beaconEditorInput.CameraManager.SwitchToModCam.performed += OnCameraManagerSwitch;

            _beaconEditorInput.Beacon.UnlockAdd.Enable();
            _beaconEditorInput.Beacon.UnlockAdd.started += ctx => _canAddBeacon = true;
            _beaconEditorInput.Beacon.UnlockAdd.canceled += ctx => _canAddBeacon = false;

            _beaconEditorInput.Beacon.Add.Enable();
            _beaconEditorInput.Beacon.Add.started += OnBeaconAdd;

            _beaconEditorInput.Beacon.MoveLock.Enable();
            _beaconEditorInput.Beacon.MoveLock.started += ctx => _canSelectedBeaconMove = true;
            _beaconEditorInput.Beacon.MoveLock.canceled += ctx => _canSelectedBeaconMove = false;

            // Beacon.Select must reside upon Canvas.UpdatePanelBeaconSelection
            // for update to take place immenently rather than in next click.
            _beaconEditorInput.Beacon.Select.Enable();
            _beaconEditorInput.Beacon.Select.started += OnBeaconSelect;
            // Do not seperate block.
            _beaconEditorInput.CanvasManager.UpdatePanelBeaconSelection.Enable();
            _beaconEditorInput.CanvasManager.UpdatePanelBeaconSelection.performed += OnUpdatePanelBeaconSelection;

            _beaconEditorInput.CanvasManager.ToggleEscMenu.Enable();
            _beaconEditorInput.CanvasManager.ToggleEscMenu.performed += OnToggleEscMenu;

            _beaconEditorInput.CameraManager.Zoom.Enable();
            _beaconEditorInput.CameraManager.Zoom.performed += OnCameraManagerZoom;

            _beaconEditorInput.CameraManager.MoveLock.Enable();
            _beaconEditorInput.CameraManager.MoveLock.performed += ctx => _canCameraMove = true;
            _beaconEditorInput.CameraManager.MoveLock.canceled += ctx => _canCameraMove = false;

            _beaconEditorInput.CameraManager.Move.Enable();
            _beaconEditorInput.CameraManager.Move.performed += OnCameraManagerMove;
        }

        private void OnDisable()
        {
            _beaconEditorInput.BeaconEditorManager.ToggleActive.Disable();
            _beaconEditorInput.BeaconEditorManager.ToggleActive.performed -= OnToggleActive;

            _beaconEditorInput.Beacon.Delete.Disable();
            _beaconEditorInput.Beacon.Delete.performed -= OnBeaconDelete;

            _beaconEditorInput.Beacon.UnlockAdd.Disable();
            _beaconEditorInput.Beacon.UnlockAdd.started -= ctx => _canAddBeacon = true;
            _beaconEditorInput.Beacon.UnlockAdd.canceled -= ctx => _canAddBeacon = false;

            _beaconEditorInput.Beacon.Add.Disable();
            _beaconEditorInput.Beacon.Add.started -= OnBeaconAdd;

            _beaconEditorInput.Beacon.MoveLock.Disable();
            _beaconEditorInput.Beacon.MoveLock.started -= ctx => _canSelectedBeaconMove = true;
            _beaconEditorInput.Beacon.MoveLock.canceled -= ctx => _canSelectedBeaconMove = false;

            // See comment at Enable counterpart.
            _beaconEditorInput.Beacon.Select.Disable();
            _beaconEditorInput.Beacon.Select.started -= OnBeaconSelect;
            // Do not seperate block.
            _beaconEditorInput.CanvasManager.UpdatePanelBeaconSelection.Disable();
            _beaconEditorInput.CanvasManager.UpdatePanelBeaconSelection.performed -= OnUpdatePanelBeaconSelection;

            _beaconEditorInput.CanvasManager.ToggleEscMenu.Disable();
            _beaconEditorInput.CanvasManager.ToggleEscMenu.performed -= OnToggleEscMenu;

            _beaconEditorInput.CameraManager.SwitchToModCam.Disable();
            _beaconEditorInput.CameraManager.SwitchToModCam.performed -= OnCameraManagerSwitch;

            _beaconEditorInput.CameraManager.Zoom.Disable();
            _beaconEditorInput.CameraManager.Zoom.performed -= OnCameraManagerZoom;

            _beaconEditorInput.CameraManager.MoveLock.Disable();
            _beaconEditorInput.CameraManager.MoveLock.performed -= ctx => _canCameraMove = true;
            _beaconEditorInput.CameraManager.MoveLock.canceled -= ctx => _canCameraMove = false;

            _beaconEditorInput.CameraManager.Move.Disable();
            _beaconEditorInput.CameraManager.Move.performed -= OnCameraManagerMove;
        }

        private void OnBeaconSelect(InputAction.CallbackContext context)
        {
            SelectBeacon();
        }

        private void OnUpdatePanelBeaconSelection(InputAction.CallbackContext context)
        {
            OnSelectedBeaconChanged();
        }

        private void OnBeaconDelete(InputAction.CallbackContext context)
        {
            DeleteSelectedBeacon();
        }

        private void OnBeaconAdd(InputAction.CallbackContext context)
        {
            AddBeacon();
        }

        private void OnToggleEscMenu(InputAction.CallbackContext context)
        {
            ToggleEscMenu();
        }

        protected void OnToggleActive(InputAction.CallbackContext context)
        {
            _areFunctionalitiesActive = !_areFunctionalitiesActive;
        }

        private void OnCameraManagerSwitch(InputAction.CallbackContext context)
        {
            SwitchCamera();
        }

        private void OnCameraManagerMove(InputAction.CallbackContext context)
        {
            MoveCamera(context.ReadValue<Vector2>());
        }

        private void OnCameraManagerZoom(InputAction.CallbackContext context)
        {
            // Returns multiples of positive/negative 120.0f
            ZoomCameraLinear(context.ReadValue<float>());
        }
    }
}