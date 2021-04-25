using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core;
using Primus.ModTool.Core;
using Primus.ModTool.Functionality;
using Primus.Sample.ModTool.BeaconEditor.IO;

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
            _primusInput.BeaconEditorManager.ToggleActive.Enable();
            _primusInput.BeaconEditorManager.ToggleActive.performed += ToggleActive;

            _primusInput.Beacon.Delete.Enable();
            _primusInput.Beacon.Delete.performed += DeleteSelectedBeacon;

            _primusInput.CameraManager.SwitchToModCam.Enable();
            _primusInput.CameraManager.SwitchToModCam.performed += SwitchCam;

            _primusInput.Beacon.UnlockAdd.Enable();
            _primusInput.Beacon.UnlockAdd.started += ctx => _canAddBeacon = true;
            _primusInput.Beacon.UnlockAdd.canceled += ctx => _canAddBeacon = false;

            _primusInput.Beacon.Add.Enable();
            _primusInput.Beacon.Add.started += AddBeacon;

            _primusInput.Beacon.MoveLock.Enable();
            _primusInput.Beacon.MoveLock.started += ctx => _canSelectedBeaconMove = true;
            _primusInput.Beacon.MoveLock.canceled += ctx => _canSelectedBeaconMove = false;

            // Beacon.Select must reside upon Canvas.UpdatePanelBeaconSelection
            // for update to take place immenently rather than in next click.
            _primusInput.Beacon.Select.Enable();
            _primusInput.Beacon.Select.started += SelectBeacon;
            // Do not seperate block.
            _primusInput.CanvasManager.UpdatePanelBeaconSelection.Enable();
            _primusInput.CanvasManager.UpdatePanelBeaconSelection.performed += UpdateSelectedBeaconPanel;

            _primusInput.CanvasManager.ToggleEscMenu.Enable();
            _primusInput.CanvasManager.ToggleEscMenu.performed += ToggleEscMenu;

            _primusInput.CameraManager.Zoom.Enable();
            _primusInput.CameraManager.Zoom.performed += ZoomCameraLinear;

            _primusInput.CameraManager.MoveLock.Enable();
            _primusInput.CameraManager.MoveLock.performed += ctx => _canCameraMove = true;
            _primusInput.CameraManager.MoveLock.canceled += ctx => _canCameraMove = false;

            _primusInput.CameraManager.Move.Enable();
            _primusInput.CameraManager.Move.performed += MoveCamera;
        }

        private void OnDisable()
        {
            _primusInput.BeaconEditorManager.ToggleActive.Disable();
            _primusInput.BeaconEditorManager.ToggleActive.performed -= ToggleActive;

            _primusInput.Beacon.Delete.Disable();
            _primusInput.Beacon.Delete.performed -= DeleteSelectedBeacon;

            _primusInput.Beacon.UnlockAdd.Disable();
            _primusInput.Beacon.UnlockAdd.started -= ctx => _canAddBeacon = true;
            _primusInput.Beacon.UnlockAdd.canceled -= ctx => _canAddBeacon = false;

            _primusInput.Beacon.Add.Disable();
            _primusInput.Beacon.Add.started -= AddBeacon;

            _primusInput.Beacon.MoveLock.Disable();
            _primusInput.Beacon.MoveLock.started -= ctx => _canSelectedBeaconMove = true;
            _primusInput.Beacon.MoveLock.canceled -= ctx => _canSelectedBeaconMove = false;

            // See comment at Enable counterpart.
            _primusInput.Beacon.Select.Disable();
            _primusInput.Beacon.Select.started -= SelectBeacon;
            // Do not seperate block.
            _primusInput.CanvasManager.UpdatePanelBeaconSelection.Disable();
            _primusInput.CanvasManager.UpdatePanelBeaconSelection.performed -= UpdateSelectedBeaconPanel;

            _primusInput.CanvasManager.ToggleEscMenu.Disable();
            _primusInput.CanvasManager.ToggleEscMenu.performed -= ToggleEscMenu;

            _primusInput.CameraManager.SwitchToModCam.Disable();
            _primusInput.CameraManager.SwitchToModCam.performed -= SwitchCam;

            _primusInput.CameraManager.Zoom.Disable();
            _primusInput.CameraManager.Zoom.performed -= ZoomCameraLinear;

            _primusInput.CameraManager.MoveLock.Disable();
            _primusInput.CameraManager.MoveLock.performed -= ctx => _canCameraMove = true;
            _primusInput.CameraManager.MoveLock.canceled -= ctx => _canCameraMove = false;

            _primusInput.CameraManager.Move.Disable();
            _primusInput.CameraManager.Move.performed -= MoveCamera;
        }
    }
}