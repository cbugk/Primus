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
        [SerializeField] private BackgroundQuad _backgroundQuad;
        private CursorManager _cursorManager { get; set; }
        [SerializeField] private CanvasManager _canvasManager;
        [SerializeField] private CameraManager _cameraManager;
        // Mouse-wheel returns +-120f so 0.01f is a good ZoomMultiplier.
        [SerializeField] private float _zoomMultiplierOrthographic = 3.0f;
        [SerializeField] private float _zoomMultiplierPerspective = 0.1f;
        // Set just bigger than half the radius of cylinder prefab.
        [SerializeField] private float _neighborBeaconDistanceLowerLimit = 25.0f;
        [SerializeField] private float _neighborBeaconDistanceEpsilon = 0.01f;
        public BeaconType BeaconTypeDropdownSelected { get; set; }
        // List of all the beacons in scene.
        [SerializeField] internal List<GameObject> BeaconInstances;


        // Private, and Protected //
        private BeaconEditorInput _beaconEditorInput;
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
        // Coupled with indNeighborBeaconsInPosition().
        private List<GameObject> _neighborBeaconInstancesCache;
        // Coupled with indNeighborBeaconsInPosition().
        private List<float> _neighborBeaconInstancesDistanceCache;
        // Instance cache for new beacon checkouts, is set to null when not in use.
        private GameObject _beaconInstanceCache;
        private GameObject _selectedBeaconInstance = null;

        private Vector3 _selectionOffsetCache;
        [Header("Layers")] [SerializeField] private LayerMask _layerBackground;
        [SerializeField] private LayerMask _layerBeacon;

        protected override void Awake()
        {
            base.Awake();

            BeaconInstances = new List<GameObject>();
            _neighborBeaconInstancesCache = new List<GameObject>();
            _neighborBeaconInstancesDistanceCache = new List<float>();
            _areFunctionalitiesActive = false;
            _canCameraMove = false;
            _canSelectedBeaconMove = false;
            _beaconEditorInput = new BeaconEditorInput();

            //Instantiate NonSerialized Functionalities
            _cursorManager = new CursorManager();
        }

        private void Start()
        {
            if (_cameraManager != null)
            {
                // _cursorManager.ActiveCamera = _cameraManager.ActiveCamera;
            }
            if (Bibliotheca == null || _canvasManager == null || _backgroundQuad == null)
            {
                throw new System.MissingMemberException();
            }

            AddCanvasListeners();
        }

        private void Update()
        {
            DragSelectedBeacon();
        }

        private void SwitchCamera()
        {
            // Switch camera index from 0 to 1 or vice-versa.
            _cameraManager.IndexActive = _cameraManager.IndexActive == 0 ? 1 : 0;
            //_cursorManager.ActiveCamera = _cameraManager.ActiveCamera;
        }

        private void MoveCamera(Vector2 delta)
        {
            if (!_canCameraMove)
            {
                return;
            }

            Vector2 moveCache = delta * _moveMultiplier;
            _cameraManager.Move(moveCache.x, moveCache.y);
            _cameraManager.ActiveCamera.transform.position =
                _backgroundQuad.Clamp(_cameraManager.ActiveCamera.transform.position);
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

        private void SelectBeacon()
        {
            var quadruple = _cursorManager.GetHit(_layerBackground);
            // If there was UI hit
            if (quadruple.Item4)
            {
                _selectedBeaconInstance = null;
                return;
            }
            // If there was no Beacon hit
            if (quadruple.Item1.x == Mathf.Infinity) { return; }
            _selectedBeaconInstance = FindNearestBeaconWithinRadiusAtPosition(
                _neighborBeaconDistanceLowerLimit / 2.0f + _neighborBeaconDistanceEpsilon,
                quadruple.Item1
            );
            if (_selectedBeaconInstance)
            {
                _selectionOffsetCache = quadruple.Item1 - _selectedBeaconInstance.transform.position;
            }
        }

        private void DeleteSelectedBeacon()
        {
            if (_selectedBeaconInstance)
            {
                BeaconInstances.Remove(_selectedBeaconInstance);
                Bibliotheca.CheckIn(_selectedBeaconInstance.GetComponent<BaseBeacon>());
                _selectedBeaconInstance = null;
                OnSelectedBeaconChanged();
                OnBeaconInstancesChanged();
            }
        }

        private void DragSelectedBeacon()
        {
            if (_selectedBeaconInstance && _canSelectedBeaconMove)
            {
                Vector3 coordinates = _cursorManager.GetCoordinates(_layerBackground);
                if (coordinates.x == Mathf.Infinity) { return; }

                coordinates.y = 0;
                var neighbor = FindNearestBeaconWithinRadiusAtPosition(_neighborBeaconDistanceLowerLimit, coordinates, _selectedBeaconInstance);
                if (!neighbor)
                {
                    _selectedBeaconInstance.transform.position = coordinates;
                }
            }
        }

        private void AddBeacon()
        {
            if (_canAddBeacon)
            {
                Vector3 coordinates = _cursorManager.GetCoordinates(_layerBackground);
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

                    OnBeaconInstancesChanged();
                }
            }
        }

        internal void EnlistBeaconInstance(GameObject beaconInstance)
        {
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

        private void ToggleEscMenu()
        {
            _canvasManager.PanelEscMenu.gameObject.SetActive(!_canvasManager.PanelEscMenu.gameObject.activeSelf);
            _canvasManager.PanelListBeacon.gameObject.SetActive(!_canvasManager.PanelListBeacon.gameObject.activeSelf);
        }

        private void OnSelectedBeaconChanged()
        {
            _canvasManager.PanelSelectionBeacon.BeaconInstance = _selectedBeaconInstance;
        }

        public void OnBeaconInstancesChanged()
        {
            ClearAndPopulateListButtonBeacon();
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
                BeaconType type = beaconInstance.GetComponent<BaseBeacon>().BiblionTitle;
            }
        }

        private void ClearAndPopulateListButtonBeacon()
        {
            _canvasManager.PanelListBeacon.ScrollListBeacon.ClearAndPopulate();
        }

        private void ZoomCameraLinear(float value)
        {
            if (_cameraManager.ActiveCamera.orthographic)
            {
                _cameraManager.ZoomLinearOrthographic(value * _zoomMultiplierOrthographic, 200, 4000);
            }
            else
            {
                _cameraManager.ZoomLinearPerspective(value * _zoomMultiplierPerspective, 0, 400000);
            }
        }

        private void ZoomCameraExponential(float value)
        {
            float zoomMultiplierExponential = 1.02f;
            int contextQuant = 120;

            // value represents scaleFactor within if statement.
            // Sets scale factor to specified ratio for zoom-in, or inverse of it for zoom-out.
            if (value < 0)
            {
                value = -value;
                value = Mathf.Pow(zoomMultiplierExponential, ((int)value / contextQuant));
                value = 1 / value;
            }
            else
            {
                value = Mathf.Pow(zoomMultiplierExponential, ((int)value / contextQuant));
            }

            if (_cameraManager.ActiveCamera.orthographic)
            {
                _cameraManager.ZoomExponentialOrthographic(value);
            }
            else
            {
                _cameraManager.ZoomExponentialPerspective(value);
            }
        }
    }
}