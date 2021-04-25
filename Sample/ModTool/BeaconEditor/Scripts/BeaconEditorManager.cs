using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core;
using Primus.ModTool.Core;
using Primus.ModTool.Functionality;
using Primus.Sample.ModTool.BeaconEditor.Canvas;
using Primus.Sample.ModTool.BeaconEditor.Beacon;

namespace Primus.Sample.ModTool.BeaconEditor
{
    /// <summary>
    /// Map Editor Program's main function. It creates and manupilates all the instances in the program. 
    /// </summary>
    public partial class BeaconEditorManager : BaseMonoSingleton<BeaconEditorManager>, IFunctionalityManager
    {
        // Public, Serialized, and Internal //
        public BeaconBibliotheca Bibliotheca;
        [SerializeField] private CanvasManager _canvasManager;
        [SerializeField] private CameraManager _cameraManager;
        // Mouse-wheel returns +-120f so 0.01f is a good ZoomMultiplier.
        [SerializeField] private float _zoomMultiplierOrthographic = 3.0f;
        [SerializeField] private float _zoomMultiplierPerspective = 0.1f;
        // Set just bigger than half the radius of cylinder prefab.
        [SerializeField] private float _neighborBeaconDistanceLowerLimit = 25.0f;
        [SerializeField] private float _neighborBeaconDistanceEpsilon = 0.01f;
        [SerializeField] private GameObject _background;
        public BeaconType BeaconTypeDropdownSelected { get; set; }
        // List of all the beacons in scene.
        [SerializeField] internal List<GameObject> BeaconInstances;


        // Private, and Protected //
        private PrimusInput _primusInput;
        protected bool _areFunctionalitiesActive;
        private bool _isMainCameraActive;
        private bool _canCameraMove;
        private bool _canAddBeacon;
        private bool _canSelectedBeaconMove;
        private float _moveMultiplier
        {
            get
            {
                return (_cameraManager.IsActiveOrthographic ? _cameraManager.ActiveOrthographicSize : _cameraManager.ActiveFieldOfView * 2.0f) * 2.5f / 1080;
            }
        }
        private MouseToWorldPosition _mouseToWorldPosition;
        // Coupled with indNeighborBeaconsInPosition().
        private List<GameObject> _neighborBeaconInstancesCache;
        // Coupled with indNeighborBeaconsInPosition().
        private List<float> _neighborBeaconInstancesDistanceCache;
        // Instance cache for new beacon checkouts, is set to null when not in use.
        private GameObject _beaconInstanceCache;
        [SerializeField] private GameObject _selectedBeaconInstance;

        protected override void Awake()
        {
            base.Awake();

            BeaconInstances = new List<GameObject>();
            _neighborBeaconInstancesCache = new List<GameObject>();
            _neighborBeaconInstancesDistanceCache = new List<float>();
            _areFunctionalitiesActive = false;
            _canCameraMove = false;
            _canSelectedBeaconMove = false;
            _primusInput = new PrimusInput();

            //Instantiate NonSerialized Functionalities
            _mouseToWorldPosition = new MouseToWorldPosition(_primusInput);
            if (_cameraManager != null)
            {
                _cameraManager.ManualAwake();
                _mouseToWorldPosition.ActiveCamera = _cameraManager.ActiveCamera;
            }
        }

        private void Start()
        {
            if (Bibliotheca == null || _canvasManager == null)
            {
                throw new System.MissingMemberException();
            }

            AddCanvasListeners();
        }
        private void Update()
        {
            DragSelectedBeacon();
        }

        protected void ToggleActive(InputAction.CallbackContext context)
        {
            _areFunctionalitiesActive = !_areFunctionalitiesActive;
        }

        private void SwitchCam(InputAction.CallbackContext context)
        {
            // Always switches to other than active camera
            _cameraManager.SwitchTo(1);
            _mouseToWorldPosition.ActiveCamera = _cameraManager.ActiveCamera;

        }

        private void MoveCamera(InputAction.CallbackContext context)
        {
            if (!_canCameraMove)
            {
                return;
            }

            Vector2 moveCache = context.ReadValue<Vector2>() * _moveMultiplier;
            _cameraManager.Move(moveCache.x, moveCache.y);
        }

        private void ZoomCameraLinear(InputAction.CallbackContext context)
        {
            // Returns positive/negative multiples of 120.0f
            float contextValue = context.ReadValue<float>();

            if (_cameraManager.ActiveCamera.orthographic)
            {
                _cameraManager.ZoomLinearOrthographic(contextValue * _zoomMultiplierOrthographic, 200, 4000);
            }
            else
            {
                _cameraManager.ZoomLinearPerspective(contextValue * _zoomMultiplierPerspective, 0, 400000);
            }
        }

        private GameObject FindNearestBeaconWithinRadiusAtPosition(float radius, Vector3 position, GameObject exceptBeaconInstance = null)
        {
            FindNeighborBeaconsWithinRadiusAtPosition(radius, position, exceptBeaconInstance);

            if (0 < _neighborBeaconInstancesCache.Count & _neighborBeaconInstancesDistanceCache.Count == _neighborBeaconInstancesCache.Count)
            {
                int leastDistant = 0;
                float leastDistance = _neighborBeaconInstancesDistanceCache[0];
                for (int index = 1; index < _neighborBeaconInstancesDistanceCache.Count; index++)
                {
                    if (_neighborBeaconInstancesDistanceCache[index] < leastDistance)
                    {
                        leastDistance = _neighborBeaconInstancesDistanceCache[index];
                        leastDistant = index;
                    }
                }

                return _neighborBeaconInstancesCache[leastDistant];
            }
            return null;
        }

        private void FindNeighborBeaconsWithinRadiusAtPosition(float radius, Vector3 position, GameObject referenceBeaconInstance)
        {
            _neighborBeaconInstancesCache.Clear();
            _neighborBeaconInstancesDistanceCache.Clear();

            foreach (var beaconInstance in BeaconInstances)
            {
                if (beaconInstance == referenceBeaconInstance)
                {
                    continue;
                }

                float distance = (position - beaconInstance.transform.position).magnitude;
                if (distance < radius)
                {
                    _neighborBeaconInstancesCache.Add(beaconInstance);
                    _neighborBeaconInstancesDistanceCache.Add(distance);
                }
            }
        }

        private void SelectBeacon(InputAction.CallbackContext context)
        {
            Vector3 coordinates = _mouseToWorldPosition.GetCoordinates();
            // If there was no hit
            if (coordinates.x == Mathf.Infinity) { return; }
            _selectedBeaconInstance = FindNearestBeaconWithinRadiusAtPosition(_neighborBeaconDistanceLowerLimit / 2.0f + _neighborBeaconDistanceEpsilon, coordinates);
        }

        private void DeleteSelectedBeacon(InputAction.CallbackContext context)
        {
            BeaconInstances.Remove(_selectedBeaconInstance);
            Bibliotheca.CheckIn(_selectedBeaconInstance.GetComponent<BaseBeacon>());
            _selectedBeaconInstance = null;
        }

        //private void DragSelectedBeacon(InputAction.CallbackContext context)
        private void DragSelectedBeacon()
        {
            if (_selectedBeaconInstance && _canSelectedBeaconMove)
            {
                Vector3 coordinates = _mouseToWorldPosition.GetCoordinates();
                if (coordinates.x == Mathf.Infinity) { return; }

                coordinates.y = 0;
                var neighbor = FindNearestBeaconWithinRadiusAtPosition(_neighborBeaconDistanceLowerLimit, coordinates, _selectedBeaconInstance);
                if (!neighbor)
                {
                    _selectedBeaconInstance.transform.position = coordinates;
                }
            }
        }

        private void AddBeacon(InputAction.CallbackContext context)
        {
            if (_canAddBeacon)
            {
                Vector3 coordinates = _mouseToWorldPosition.GetCoordinates();
                // Height is standardized to zero
                coordinates.y = 0;

                // If there were no hit.
                if (coordinates.x == Mathf.Infinity)
                {
                    return;
                }
                // If there exists a selection candidate instead.
                if (FindNearestBeaconWithinRadiusAtPosition(_neighborBeaconDistanceLowerLimit / 2.0f + _neighborBeaconDistanceEpsilon, coordinates))
                {
                    return;
                }

                _beaconInstanceCache = Bibliotheca.CheckOut(BeaconTypeDropdownSelected);
                if (_beaconInstanceCache)
                {
                    _beaconInstanceCache.transform.position = coordinates;
                    EnlistBeaconInstance(_beaconInstanceCache);

                    // Keep cache empty when not in use.
                    _beaconInstanceCache = null;
                }
            }
        }

        internal void EnlistBeaconInstance(GameObject beaconInstance)
        {
            // All instances are children of the background to be able to shifted when importing.
            //beaconInstance.transform.SetParent(_background.transform);
            if (beaconInstance)
            {
                BeaconInstances.Add(beaconInstance);
            }
            else
            {
                Debug.Log("Beacon instance is null!");
            }
        }

        public void ClearBeacons()
        {
            foreach (var beaconInstance in BeaconInstances)
            {
                Bibliotheca.CheckIn(beaconInstance.GetComponent<BaseBeacon>());
            }

            BeaconInstances.Clear();
        }

        private void ToggleEscMenu(InputAction.CallbackContext context)
        {
            _canvasManager.PanelEscMenu.gameObject.SetActive(!_canvasManager.PanelEscMenu.gameObject.activeSelf);
            _canvasManager.PanelListBeacon.gameObject.SetActive(!_canvasManager.PanelListBeacon.gameObject.activeSelf);
        }
        private void UpdateSelectedBeaconPanel(InputAction.CallbackContext context)
        {
            _canvasManager.PanelSelectionBeacon.BeaconInstance = _selectedBeaconInstance;
        }

        private void SaveState()
        {
            IO.StateIO.Write();
        }
        private void LoadState()
        {
            IO.StateIO.Read("Save01.bin");
            foreach (var beaconInstance in BeaconInstances)
            {
                BeaconType type = beaconInstance.GetComponent<BaseBeacon>().Title;
            }
        }
        private void ClearAndPopulateListButtonBeacon()
        {
            _canvasManager.PanelListBeacon.ScrollListBeacon.ClearAndPopulate();
        }

        private void ZoomCameraExponential(InputAction.CallbackContext context)
        {
            float zoomMultiplierExponential = 1.02f;

            // Returns positive/negative multiples of 120.0f
            float contextValue = context.ReadValue<float>();
            int contextQuant = 120;

            // contextValue represents scaleFactor within if statement.
            // Sets scale factor to specified ratio for zoom-in, or inverse of it for zoom-out.
            if (contextValue < 0)
            {
                contextValue = -contextValue;
                contextValue = Mathf.Pow(zoomMultiplierExponential, ((int)contextValue / contextQuant));
                contextValue = 1 / contextValue;
            }
            else
            {
                contextValue = Mathf.Pow(zoomMultiplierExponential, ((int)contextValue / contextQuant));
            }

            if (_cameraManager.ActiveCamera.orthographic)
            {
                _cameraManager.ZoomExponentialOrthographic(contextValue);
            }
            else
            {
                _cameraManager.ZoomExponentialPerspective(contextValue);
            }
        }
    }
}