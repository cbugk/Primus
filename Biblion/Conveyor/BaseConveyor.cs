using System;
using UnityEngine;
using Primus.Core.Bibliotheca;

namespace Primus.Biblion.Conveyor
{
    [ExecuteInEditMode]
    public abstract class BaseConveyor<TCatalogTitle> : BaseBiblion<TCatalogTitle> where TCatalogTitle : Enum
    {
        // Kill switch (let's conveyor remember last set _velocityMultiplier)
        [field: SerializeField] public bool DoRun { get; set; }
        [field: SerializeField] public float MultiplierVelocity { get; set; }
        [SerializeField] private float _width;
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
            }
        }
        [SerializeField] private float _length;
        public float Length
        {
            get => _length;
            set
            {
                _length = value;
                OnDimensionChanged();
            }
        }
        [SerializeField] private float _diameter;
        public float Diameter
        {
            get => _diameter;
            set
            {
                _diameter = value;
                OnDimensionChanged();
            }
        }
        [SerializeField] private float _beltThicknessToDiameterRatio = 0.1f;

        [SerializeField] private bool _doCreateBeltBelow = true;

        // Length and diameter are calculated in terms of one another (and tile numbers)
        [SerializeField] private bool _isPrimaryLengthNotDiameter = true;



        // Object for sourcing rotation, position and some size information
        [SerializeField] private int _numberBeltTiles = 10;
        [SerializeField] private int _numberRollerTiles = 1;
        [SerializeField] private float _multiplierRollerWidth = 1.0f;
        private float _accumulatedOffset;

        // BELTS.
        [SerializeField] private GameObject _beltAboveLeft;
        [SerializeField] private GameObject _beltAboveRight;
        [SerializeField] private GameObject _beltAboveUp;
        [SerializeField] private GameObject _beltBelowLeft;
        [SerializeField] private GameObject _beltBelowRight;
        [SerializeField] private GameObject _beltBelowUp;
        private Rigidbody _beltBodyAboveLeft;
        private Rigidbody _beltBodyAboveRight;
        private Rigidbody _beltBodyAboveUp;
        private Rigidbody _beltBodyBelowLeft;
        private Rigidbody _beltBodyBelowRight;
        private Rigidbody _beltBodyBelowUp;
        private Vector2 _beltMaterialOffset = Vector2.zero;
        private Renderer _beltRendererAboveLeft;
        private Renderer _beltRendererAboveRight;
        private Renderer _beltRendererAboveUp;
        private Renderer _beltRendererBelowLeft;
        private Renderer _beltRendererBelowRight;
        private Renderer _beltRendererBelowUp;
        private Vector3 _beltStep = Vector3.zero;
        private float _beltThickness;
        private Transform _beltTransformAboveLeft;
        private Transform _beltTransformAboveRight;
        private Transform _beltTransformAboveUp;
        private Transform _beltTransformBelowLeft;
        private Transform _beltTransformBelowRight;
        private Transform _beltTransformBelowUp;

        // ROLLERS
        [SerializeField] private GameObject _rollerFront;
        [SerializeField] private GameObject _rollerHind;
        private Renderer _rollerRendererFront;
        private Renderer _rollerRendererHind;
        private Transform _rollerTransformFront;
        private Transform _rollerTransformHind;
        private float _tileLength;

        // Caching members
        private float _deltaVelocityTime;

        [SerializeField] private Material _materialBeltSide;
        [SerializeField] private Material _materialBeltUp;

        [SerializeField] private Material _materialRoller;
        private int _numberRollerTilesTwice;
        private Rigidbody _rollerBodyFront;
        private Rigidbody _rollerBodyHind;
        private Quaternion _rollerDeltaRotation;
        private Vector3 _rollerEulerAnglePerTile = Vector3.zero;

        //UpdteStateCache
        private Vector3 partTransformLocalPositionCache = Vector3.zero;
        private Vector3 partTransformLocalScaleCache = Vector3.forward;



        private void Start()
        {
            SetPartReferences();
            OnDimensionChanged();
        }

        // Governing surface and physics
        private void FixedUpdate()
        {
            FixedUpdatePhysicsAndRendering();
        }

        private void Update()
        {
#if UNITY_EDITOR
            // Reshapes biblion within editor. 
            OnDimensionChanged();
#endif
        }

        private void OnDimensionChanged()
        {
            CalculateDimensionVariables();
            UpdateStates();
        }

        /// <summary>Calculates dependent values whenever a change is made.</summary>
        private void CalculateDimensionVariables()
        {
            // Cylinder wraps same tile twice, adjusting tile number accordingly.
            _numberRollerTilesTwice = _numberRollerTiles * 2;

            if (_numberBeltTiles.Equals(0) || _numberRollerTilesTwice.Equals(0))
            {
                Debug.Log("Tile numbers for Belt and Roller cannot be zero");
                gameObject.SetActive(false);
                return;
            }

            if (_isPrimaryLengthNotDiameter)
            {
                _length = Length;
                _tileLength = _length / _numberBeltTiles;
                _diameter = _tileLength * _numberRollerTilesTwice / Mathf.PI;
            }
            else
            {
                _diameter = Diameter;
                _tileLength = _diameter * Mathf.PI / _numberRollerTilesTwice;
                _length = _tileLength * _numberBeltTiles;
            }

            // After _diameter is decided upon
            _beltThickness = _beltThicknessToDiameterRatio * _diameter;

            _rollerEulerAnglePerTile.y = 360.0f / _numberRollerTilesTwice;
        }

        private void FixedUpdatePhysicsAndRendering()
        {
            if (!DoRun) return;

            _deltaVelocityTime = MultiplierVelocity * Time.deltaTime;

            _beltStep = transform.forward * _deltaVelocityTime;

            // Material Offset, 0 <= _accumulatedOffset < _tileLength.
            _accumulatedOffset -= _deltaVelocityTime;
            //_accumulatedOffset %= _tileLength;
            _beltMaterialOffset.y = _accumulatedOffset;

            // Angle per _deltaVelocityTime displacement
            _rollerDeltaRotation = Quaternion.Euler(_rollerEulerAnglePerTile * _deltaVelocityTime);
            // Turn roller front
            _rollerBodyFront.MoveRotation(_rollerBodyFront.rotation * _rollerDeltaRotation);
            // Turn roller hind
            _rollerBodyHind.MoveRotation(_rollerBodyHind.rotation * _rollerDeltaRotation);

            // Move _beltAbove forward (no physics)
            _beltBodyAboveUp.position -= _beltStep;
            _beltBodyAboveRight.position -= _beltStep;
            _beltBodyAboveLeft.position -= _beltStep;
            // Move _beltAbove back (rigidbody)
            _beltBodyAboveUp.MovePosition(_beltBodyAboveUp.position + _beltStep);
            _beltBodyAboveRight.MovePosition(_beltBodyAboveRight.position + _beltStep);
            _beltBodyAboveLeft.MovePosition(_beltBodyAboveLeft.position + _beltStep);
            // Move material accordingly
            _beltRendererAboveUp.sharedMaterial.mainTextureOffset = _beltMaterialOffset;
            _beltRendererAboveRight.sharedMaterial.mainTextureOffset = _beltMaterialOffset;
            _beltRendererAboveLeft.sharedMaterial.mainTextureOffset = _beltMaterialOffset;

            if (_doCreateBeltBelow)
            {
                // Move _beltBelow forward (no physics)
                _beltBodyBelowUp.position += _beltStep;
                _beltBodyBelowRight.position += _beltStep;
                _beltBodyBelowLeft.position += _beltStep;
                // Move _beltBelow back (rigidbody)
                _beltBodyBelowUp.MovePosition(_beltBodyBelowUp.position - _beltStep);
                _beltBodyBelowRight.MovePosition(_beltBodyBelowRight.position - _beltStep);
                _beltBodyBelowLeft.MovePosition(_beltBodyBelowLeft.position - _beltStep);
                // Move tiled material accordingly
                _beltRendererBelowUp.sharedMaterial.mainTextureOffset = _beltMaterialOffset;
                _beltRendererBelowRight.sharedMaterial.mainTextureOffset = _beltMaterialOffset;
                _beltRendererBelowLeft.sharedMaterial.mainTextureOffset = _beltMaterialOffset;
            }
        }

        // HEREAFTER functions for setting part caches.
        ///<summary>Sets caches for belts and rollers.</summary>
        private void SetPartReferences()
        {
            SetReferencesBeltPart(_beltAboveLeft, out _beltBodyAboveLeft, out _beltRendererAboveLeft,
                out _beltTransformAboveLeft);
            SetReferencesBeltPart(_beltAboveUp, out _beltBodyAboveUp, out _beltRendererAboveUp, out _beltTransformAboveUp);
            SetReferencesBeltPart(_beltAboveRight, out _beltBodyAboveRight, out _beltRendererAboveRight,
                out _beltTransformAboveRight);

            if (_doCreateBeltBelow)
            {
                SetReferencesBeltPart(_beltBelowLeft, out _beltBodyBelowLeft, out _beltRendererBelowLeft,
                    out _beltTransformBelowLeft);
                SetReferencesBeltPart(_beltBelowUp, out _beltBodyBelowUp, out _beltRendererBelowUp, out _beltTransformBelowUp);
                SetReferencesBeltPart(_beltBelowRight, out _beltBodyBelowRight, out _beltRendererBelowRight,
                    out _beltTransformBelowRight);
            }

            SetReferencesRoller(_rollerFront, out _rollerBodyFront, out _rollerRendererFront, out _rollerTransformFront);

            SetReferencesRoller(_rollerHind, out _rollerBodyHind, out _rollerRendererHind, out _rollerTransformHind);
        }

        ///<summary>Sets caches for a belt part.</summary>
        private void SetReferencesBeltPart(in GameObject beltPart, out Rigidbody beltBody, out Renderer beltRenderer,
            out Transform beltTransform)
        {
            beltBody = beltPart.GetComponent<Rigidbody>();
            beltBody.isKinematic = true;

            beltRenderer = beltPart.GetComponent<Renderer>();

            beltTransform = beltPart.transform;
        }
        ///<summary>Sets caches for a roller.</summary>
        private void SetReferencesRoller(in GameObject roller, out Rigidbody rollerBody, out Renderer rollerRenderer,
            out Transform rollerTransform)
        {
            rollerBody = roller.GetComponent<Rigidbody>();
            rollerBody.isKinematic = true;

            rollerRenderer = roller.GetComponent<Renderer>();

            rollerTransform = roller.transform;
            rollerTransform.rotation = transform.rotation;
            // Make roller's y direction look transform.right
            rollerTransform.localEulerAngles = new Vector3(0, 0, -90);
        }

        // HEREAFTER functions for setting part caches.
        ///<summary>Updates belt and roller states.</summary>
        private void UpdateStates()
        {
            UpdateStateBelt(BeltPartType.ABOVE_LEFT, _beltRendererAboveLeft, _beltTransformAboveLeft);
            UpdateStateBelt(BeltPartType.ABOVE_MIDDLE, _beltRendererAboveUp, _beltTransformAboveUp);
            UpdateStateBelt(BeltPartType.ABOVE_RIGHT, _beltRendererAboveRight, _beltTransformAboveRight);

            if (_doCreateBeltBelow)
            {
                UpdateStateBelt(BeltPartType.BELOW_LEFT, _beltRendererBelowLeft, _beltTransformBelowLeft);
                UpdateStateBelt(BeltPartType.BELOW_MIDDLE, _beltRendererBelowUp, _beltTransformBelowUp);
                UpdateStateBelt(BeltPartType.BELOW_RIGHT, _beltRendererBelowRight, _beltTransformBelowRight);
            }

            UpdateStateRoller(RollerType.FRONT, _rollerRendererFront, _rollerTransformFront);

            UpdateStateRoller(RollerType.HIND, _rollerRendererHind, _rollerTransformHind);
        }

        ///<summary>Updates position, rotation, local scale, and material of belt.</summary>
        private void UpdateStateBelt(BeltPartType partType, Renderer beltRenderer, Transform beltTransform)
        {
            if (beltRenderer == null || beltTransform == null) { return; }

            switch (partType)
            {
                case BeltPartType.ABOVE_LEFT:
                    partTransformLocalPositionCache.x = -_width / 2.0f;
                    partTransformLocalPositionCache.y = (_diameter - _beltThickness) / 2.0f;
                    partTransformLocalPositionCache.z = 0.0f;
                    beltTransform.localPosition = partTransformLocalPositionCache;
                    // Norm is conveyor's left
                    beltTransform.localRotation = Quaternion.Euler(0, 90, 90);
                    partTransformLocalScaleCache.x = _beltThickness;
                    partTransformLocalScaleCache.y = _length - _diameter;
                    partTransformLocalScaleCache.z = 1.0f;
                    beltTransform.localScale = partTransformLocalScaleCache;
                    if (!beltRenderer.sharedMaterial) beltRenderer.sharedMaterial = _materialBeltSide;
                    break;
                case BeltPartType.ABOVE_MIDDLE:
                    partTransformLocalPositionCache.x = 0.0f;
                    partTransformLocalPositionCache.y = _diameter / 2.0f;
                    partTransformLocalPositionCache.z = 0.0f;
                    beltTransform.localPosition = partTransformLocalPositionCache;
                    // Norm is conveyor's up
                    beltTransform.localRotation = Quaternion.Euler(90, 0, 0);
                    partTransformLocalScaleCache.x = _width;
                    partTransformLocalScaleCache.y = _length - _diameter;
                    partTransformLocalScaleCache.z = 1.0f;
                    beltTransform.localScale = partTransformLocalScaleCache;
                    if (!beltRenderer.sharedMaterial) beltRenderer.sharedMaterial = _materialBeltUp;
                    break;
                case BeltPartType.ABOVE_RIGHT:
                    partTransformLocalPositionCache.x = _width / 2.0f;
                    partTransformLocalPositionCache.y = (_diameter - _beltThickness) / 2.0f;
                    partTransformLocalPositionCache.z = 0.0f;
                    beltTransform.localPosition = partTransformLocalPositionCache;
                    // Norm is conveyor's right
                    beltTransform.localRotation = Quaternion.Euler(0, -90, -90);
                    partTransformLocalScaleCache.x = _beltThickness;
                    partTransformLocalScaleCache.y = _length - _diameter;
                    partTransformLocalScaleCache.z = 1.0f;
                    beltTransform.localScale = partTransformLocalScaleCache;
                    if (!beltRenderer.sharedMaterial) beltRenderer.sharedMaterial = _materialBeltSide;
                    break;
                case BeltPartType.BELOW_LEFT:
                    partTransformLocalPositionCache.x = -_width / 2.0f;
                    partTransformLocalPositionCache.y = -(_diameter - _beltThickness) / 2.0f;
                    partTransformLocalPositionCache.z = 0.0f;
                    beltTransform.localPosition = partTransformLocalPositionCache;
                    // Norm is conveyor's left
                    beltTransform.localRotation = Quaternion.Euler(0, 90, -90);
                    partTransformLocalScaleCache.x = _beltThickness;
                    partTransformLocalScaleCache.y = _length - _diameter;
                    partTransformLocalScaleCache.z = 1.0f;
                    beltTransform.localScale = partTransformLocalScaleCache;
                    if (!beltRenderer.sharedMaterial) beltRenderer.sharedMaterial = _materialBeltSide;
                    break;
                case BeltPartType.BELOW_MIDDLE:
                    partTransformLocalPositionCache.x = 0.0f;
                    partTransformLocalPositionCache.y = -_diameter / 2.0f;
                    partTransformLocalPositionCache.z = 0.0f;
                    beltTransform.localPosition = partTransformLocalPositionCache;
                    // Norm is conveyor's down
                    beltTransform.localRotation = Quaternion.Euler(-90, 0, 0);
                    partTransformLocalScaleCache.x = _width;
                    partTransformLocalScaleCache.y = _length - _diameter;
                    partTransformLocalScaleCache.z = 1.0f;
                    beltTransform.localScale = partTransformLocalScaleCache;
                    if (!beltRenderer.sharedMaterial) beltRenderer.sharedMaterial = _materialBeltUp;
                    break;
                case BeltPartType.BELOW_RIGHT:
                    partTransformLocalPositionCache.x = _width / 2.0f;
                    partTransformLocalPositionCache.y = -(_diameter - _beltThickness) / 2.0f;
                    partTransformLocalPositionCache.z = 0.0f;
                    beltTransform.localPosition = partTransformLocalPositionCache;
                    // Norm is conveyor's right
                    beltTransform.localRotation = Quaternion.Euler(0, -90, 90);
                    partTransformLocalScaleCache.x = _beltThickness;
                    partTransformLocalScaleCache.y = _length - _diameter;
                    partTransformLocalScaleCache.z = 1.0f;
                    beltTransform.localScale = partTransformLocalScaleCache;
                    if (!beltRenderer.sharedMaterial) beltRenderer.sharedMaterial = _materialBeltSide;
                    break;
            }
        }

        ///<summary>Updates position, local scale, and material of roller.</summary>
        private void UpdateStateRoller(RollerType rollerType, Renderer rollerRenderer, Transform rollerTransform)
        {
            if (rollerRenderer == null || rollerTransform == null) { return; }

            float positionDotZ = Mathf.Infinity;

            switch (rollerType)
            {
                case RollerType.FRONT:
                    positionDotZ = 1.0f;
                    break;
                case RollerType.HIND:
                    positionDotZ = -1.0f;
                    break;
            }

            if (!float.IsInfinity(positionDotZ))
            {
                partTransformLocalPositionCache.x = 0.0f;
                partTransformLocalPositionCache.y = 0.0f;
                partTransformLocalPositionCache.z = positionDotZ * (_length - _diameter) / 2.0f;
                rollerTransform.localPosition = partTransformLocalPositionCache;
                partTransformLocalScaleCache.x = _diameter;
                partTransformLocalScaleCache.y = _multiplierRollerWidth * (_width / 2.0f);
                partTransformLocalScaleCache.z = _diameter;
                rollerTransform.localScale = partTransformLocalScaleCache;
            }

            if (!rollerRenderer.sharedMaterial) rollerRenderer.sharedMaterial = _materialRoller;
        }

        public enum BeltPartType
        {
            INVALID = 0,
            ABOVE_LEFT = 1,
            ABOVE_MIDDLE = 2,
            ABOVE_RIGHT = 3,
            BELOW_LEFT = 4,
            BELOW_MIDDLE = 5,
            BELOW_RIGHT = 6
        }

        public enum RollerType
        {
            INVALID = 0,
            FRONT = 1,
            HIND = 2
        }
    }
}