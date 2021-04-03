using System;
using Primus.Core.Bibliotheca;
using UnityEngine;

namespace Primus.Biblion.Conveyor
{
    [ExecuteInEditMode]
    public abstract class GenericConveyor<TCatalogTitle> : GenericBaseBiblion<TCatalogTitle> where TCatalogTitle : Enum
    {
        [SerializeField] private readonly float _beltToDiameterRatio = 0.1f;

        [SerializeField] private readonly bool _doCreateBeltBelow = true;

        // Length and diameter are calculated in terms of one another (and tile numbers)
        [SerializeField] private readonly bool _isLengthPrimary = true;

        // Object for sourcing rotation, position and some size information
        [SerializeField] private readonly int _numberBeltTiles = 10;
        [SerializeField] private readonly int _numberRollerTiles = 1;
        [SerializeField] private readonly float _rollerWidthMultiplier = 1.0f;
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

        // Caching members
        private float _deltaVelocityTime;
        private float _diameter;
        private float _length;
        [SerializeField] private Material _materialBeltSide;
        [SerializeField] private Material _materialBeltUp;

        [SerializeField] private Material _materialRoller;
        private int _numberRollerTilesTwice;
        private Rigidbody _rollerBodyFront;
        private Rigidbody _rollerBodyHind;
        private Quaternion _rollerDeltaRotation;
        private Vector3 _rollerEulerAnglePerTile = Vector3.zero;

        // ROLLERS
        [SerializeField] private GameObject _rollerFront;
        [SerializeField] private GameObject _rollerHind;
        private Renderer _rollerRendererFront;
        private Renderer _rollerRendererHind;
        private Transform _rollerTransformFront;
        private Transform _rollerTransformHind;
        private float _tileLength;

        private float _width;
        [SerializeField] public float Diameter;
        [SerializeField] public float Length;

        //UpdteStateCache
        private Vector3 partTransformLocalPositionCache = Vector3.zero;
        private Vector3 partTransformLocalScaleCache = Vector3.forward;

        [SerializeField] public float Width;

        // Kill switch (let's conveyor remember last set _velocityMultiplier)
        [field: SerializeField] public bool DoRun { get; set; }
        [field: SerializeField] public float VelocityMultiplier { get; set; }

        private void Awake()
        {
            CalculateVariables();
            SetParts();
            UpdateState();
        }

        // Governing surface and physics
        private void FixedUpdate()
        {
            CalculateVariables();
            UpdateState();
            FixedUpdatePhysicsAndRendering();
        }

        private void OnValidate()
        {
            CalculateVariables();
            UpdateState();
        }

        // Calculates values whenever a change is made
        private void CalculateVariables()
        {
            // Cylinder wraps same tile twice, adjusting tile number accordingly.
            _numberRollerTilesTwice = _numberRollerTiles * 2;

            if (_numberBeltTiles.Equals(0) || _numberRollerTilesTwice.Equals(0))
            {
                Debug.Log("Tile numbers for Belt and Roller cannot be zero");
                gameObject.SetActive(false);
                return;
            }

            _width = Width;
            if (_isLengthPrimary)
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
            _beltThickness = _beltToDiameterRatio * _diameter;

            _rollerEulerAnglePerTile.y = 360.0f / _numberRollerTilesTwice;
        }

        private void SetParts()
        {
            SetBeltPart(_beltAboveLeft, out _beltBodyAboveLeft, out _beltRendererAboveLeft,
                out _beltTransformAboveLeft);
            SetBeltPart(_beltAboveUp, out _beltBodyAboveUp, out _beltRendererAboveUp, out _beltTransformAboveUp);
            SetBeltPart(_beltAboveRight, out _beltBodyAboveRight, out _beltRendererAboveRight,
                out _beltTransformAboveRight);

            if (_doCreateBeltBelow)
            {
                SetBeltPart(_beltBelowLeft, out _beltBodyBelowLeft, out _beltRendererBelowLeft,
                    out _beltTransformBelowLeft);
                SetBeltPart(_beltBelowUp, out _beltBodyBelowUp, out _beltRendererBelowUp, out _beltTransformBelowUp);
                SetBeltPart(_beltBelowRight, out _beltBodyBelowRight, out _beltRendererBelowRight,
                    out _beltTransformBelowRight);
            }

            SetRoller(_rollerFront, out _rollerBodyFront, out _rollerRendererFront, out _rollerTransformFront);

            SetRoller(_rollerHind, out _rollerBodyHind, out _rollerRendererHind, out _rollerTransformHind);
        }

        private void SetBeltPart(in GameObject beltPart, out Rigidbody beltBody, out Renderer beltRenderer,
            out Transform beltTransform)
        {
            beltBody = beltPart.GetComponent<Rigidbody>();
            beltBody.isKinematic = true;

            beltRenderer = beltPart.GetComponent<Renderer>();

            beltTransform = beltPart.transform;
        }

        private void SetRoller(in GameObject roller, out Rigidbody rollerBody, out Renderer rollerRenderer,
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

        private void UpdateState()
        {
            UpdateBeltState(1, _beltRendererAboveLeft, _beltTransformAboveLeft);
            UpdateBeltState(2, _beltRendererAboveUp, _beltTransformAboveUp);
            UpdateBeltState(3, _beltRendererAboveRight, _beltTransformAboveRight);

            if (_doCreateBeltBelow)
            {
                UpdateBeltState(4, _beltRendererBelowLeft, _beltTransformBelowLeft);
                UpdateBeltState(5, _beltRendererBelowUp, _beltTransformBelowUp);
                UpdateBeltState(6, _beltRendererBelowRight, _beltTransformBelowRight);
            }

            UpdateRollerState(true, _rollerRendererFront, _rollerTransformFront);

            UpdateRollerState(false, _rollerRendererHind, _rollerTransformHind);
        }

        private void UpdateBeltState(int partNumber, Renderer beltRenderer, Transform beltTransform)
        {
            if (beltRenderer == null || beltTransform == null)
            {
                return;
                ;
            }

            switch (partNumber)
            {
                case 1:
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
                case 2:
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
                case 3:
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
                case 4:
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
                case 5:
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
                case 6:
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

        private void UpdateRollerState(bool isFront, Renderer rollerRenderer, Transform rollerTransform)
        {
            if (rollerRenderer == null || rollerTransform == null) return;
            partTransformLocalPositionCache.x = 0.0f;
            partTransformLocalPositionCache.y = 0.0f;
            partTransformLocalPositionCache.z = (isFront ? 1.0f : -1.0f) * (_length - _diameter) / 2.0f;
            rollerTransform.localPosition = partTransformLocalPositionCache;
            partTransformLocalScaleCache.x = _diameter;
            partTransformLocalScaleCache.y = _rollerWidthMultiplier * (_width / 2.0f);
            partTransformLocalScaleCache.z = _diameter;

            if (!rollerRenderer.sharedMaterial) rollerRenderer.sharedMaterial = _materialRoller;
        }

        private void FixedUpdatePhysicsAndRendering()
        {
            if (!DoRun) return;

            _deltaVelocityTime = VelocityMultiplier * Time.deltaTime;

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

        public override void EnterRestitutionState()
        {
            gameObject.SetActive(false);
        }

        public override void EnterCirculationState()
        {
            gameObject.SetActive(true);
        }
    }
}