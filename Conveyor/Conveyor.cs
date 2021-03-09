using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Primus.Conveyor
{
    [ExecuteInEditMode]
//    [InitializeOnLoadAttribute]
    public class Conveyor : MonoBehaviour
    {
        [SerializeField] public float Width;
        [SerializeField] public float Diameter;
        [SerializeField] public float Length;
        // Kill switch (let's conveyor remember last set _velocityMultiplier)
        [SerializeField] private bool _doRun = true;
        // Length and diameter are calculated in terms of one another (and tile numbers)
        [SerializeField] private bool _isLengthPrimary = true;
        [SerializeField] private bool _doCreateBeltBelow = true;
        [SerializeField] private float _velocityMultiplier = 1.0f;
        [SerializeField] private float _beltToDiameterRatio = 0.1f;
        [SerializeField] private float _rollerWidthMultiplier = 1.0f;
        // Object for sourcing rotation, position and some size information
        [SerializeField] private int _numberBeltTiles = 10;
        [SerializeField] private int _numberRollerTiles = 1;
        private int _numberRollerTilesTwice;
        [SerializeField] private Material _materialBeltUp;
        [SerializeField] private Material _materialBeltSide;
        [SerializeField] private Material _materialRoller;

        private float _width;
        private float _diameter;
        private float _length;
        private float _tileLength;
        private float _beltThickness;

        // BELTS.
        [SerializeField] private GameObject _beltAboveLeft;
        [SerializeField] private GameObject _beltAboveUp;
        [SerializeField] private GameObject _beltAboveRight;
        [SerializeField] private GameObject _beltBelowLeft;
        [SerializeField] private GameObject _beltBelowUp;
        [SerializeField] private GameObject _beltBelowRight;
        private Rigidbody _beltBodyAboveLeft;
        private Rigidbody _beltBodyAboveUp;
        private Rigidbody _beltBodyAboveRight;
        private Rigidbody _beltBodyBelowLeft;
        private Rigidbody _beltBodyBelowUp;
        private Rigidbody _beltBodyBelowRight;
        private Renderer _beltRendererAboveLeft;
        private Renderer _beltRendererAboveUp;
        private Renderer _beltRendererAboveRight;
        private Renderer _beltRendererBelowLeft;
        private Renderer _beltRendererBelowUp;
        private Renderer _beltRendererBelowRight;
        private Transform _beltTransformAboveLeft;
        private Transform _beltTransformAboveUp;
        private Transform _beltTransformAboveRight;
        private Transform _beltTransformBelowLeft;
        private Transform _beltTransformBelowUp;
        private Transform _beltTransformBelowRight;

        // ROLLERS
        [SerializeField] private GameObject _rollerFront;
        [SerializeField] private GameObject _rollerHind;
        private Rigidbody _rollerBodyFront;
        private Rigidbody _rollerBodyHind;
        private Renderer _rollerRendererFront;
        private Renderer _rollerRendererHind;
        private Transform _rollerTransformFront;
        private Transform _rollerTransformHind;

        // Caching members
        private float _deltaVelocityTime;
        private Vector3 _beltStep;
        private Vector3 _rollerEulerAnglePerTile;
        private Quaternion _rollerDeltaRotation;
        private float _accumulatedOffset = 0.0f;
        private Vector2 _beltMaterialOffset = new Vector2(0.0f, 0.0f);

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

            _rollerEulerAnglePerTile = new Vector3(0, (360.0f / _numberRollerTilesTwice), 0);
        }

        private void SetParts()
        {
            SetBeltPart(_beltAboveLeft, out _beltBodyAboveLeft, out _beltRendererAboveLeft, out _beltTransformAboveLeft);
            SetBeltPart(_beltAboveUp, out _beltBodyAboveUp, out _beltRendererAboveUp, out _beltTransformAboveUp);
            SetBeltPart(_beltAboveRight, out _beltBodyAboveRight, out _beltRendererAboveRight, out _beltTransformAboveRight);
            
            if (_doCreateBeltBelow)
            {
                SetBeltPart(_beltBelowLeft, out _beltBodyBelowLeft, out _beltRendererBelowLeft, out _beltTransformBelowLeft);
                SetBeltPart(_beltBelowUp, out _beltBodyBelowUp, out _beltRendererBelowUp, out _beltTransformBelowUp);
                SetBeltPart(_beltBelowRight, out _beltBodyBelowRight, out _beltRendererBelowRight, out _beltTransformBelowRight);
            }

            SetRoller(_rollerFront, out _rollerBodyFront, out _rollerRendererFront, out _rollerTransformFront);

            SetRoller(_rollerHind, out _rollerBodyHind, out _rollerRendererHind, out _rollerTransformHind);
        }

        private void SetBeltPart(in GameObject beltPart, out Rigidbody beltBody, out Renderer beltRenderer, out Transform beltTransform)
        {
            beltBody = beltPart.GetComponent<Rigidbody>();
            beltBody.isKinematic = true;

            beltRenderer = beltPart.GetComponent<Renderer>();

            beltTransform = beltPart.transform;
        }

        private void SetRoller(in GameObject roller, out Rigidbody rollerBody, out Renderer rollerRenderer, out Transform rollerTransform)
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
                return; ;
            }

            switch (partNumber)
            {
                case 1:
                    beltTransform.localPosition =
                        new Vector3(-(_width / 2.0f), ((_diameter - _beltThickness) / 2.0f), 0.0f);
                    // Norm is conveyor's left
                    beltTransform.localRotation = Quaternion.Euler(0, 90, 90);
                    beltTransform.localScale = new Vector3(_beltThickness, (_length - _diameter), 1.0f);
                    beltRenderer.material = _materialBeltSide;
                    break;
                case 2:
                    beltTransform.localPosition =
                        new Vector3(0.0f, (_diameter / 2.0f), 0.0f);
                    // Norm is conveyor's up
                    beltTransform.localRotation = Quaternion.Euler(90, 0, 0);
                    beltTransform.localScale = new Vector3(_width, (_length - _diameter), 1.0f);
                    beltRenderer.material = _materialBeltUp;
                    break;
                case 3:
                    beltTransform.localPosition =
                        new Vector3((_width / 2.0f), ((_diameter - _beltThickness) / 2.0f), 0.0f);
                    // Norm is conveyor's right
                    beltTransform.localRotation = Quaternion.Euler(0, -90, -90);
                    beltTransform.localScale = new Vector3(_beltThickness, (_length - _diameter), 1.0f);
                    beltRenderer.material = _materialBeltSide;
                    break;
                case 4:
                    beltTransform.localPosition =
                        new Vector3(-(_width / 2.0f), -((_diameter - _beltThickness) / 2.0f), 0.0f);
                    // Norm is conveyor's left
                    beltTransform.localRotation = Quaternion.Euler(0, 90, -90);
                    beltTransform.localScale = new Vector3(_beltThickness, (_length - _diameter), 1.0f);
                    beltRenderer.material = _materialBeltSide;
                    break;
                case 5:
                    beltTransform.localPosition =
                        new Vector3(0.0f, -(_diameter / 2.0f), 0.0f);
                    // Norm is conveyor's down
                    beltTransform.localRotation = Quaternion.Euler(-90, 0, 0);
                    beltTransform.localScale = new Vector3(_width, (_length - _diameter), 1.0f);
                    beltRenderer.material = _materialBeltUp;
                    break;
                case 6:
                    beltTransform.localPosition =
                        new Vector3((_width / 2.0f), -((_diameter - _beltThickness) / 2.0f), 0.0f);
                    // Norm is conveyor's right
                    beltTransform.localRotation = Quaternion.Euler(0, -90, 90);
                    beltTransform.localScale = new Vector3(_beltThickness, (_length - _diameter), 1.0f);
                    beltRenderer.material = _materialBeltSide;
                    break;
            }
        }

        private void UpdateRollerState(bool isFront, Renderer rollerRenderer, Transform rollerTransform)
        {
            if (rollerRenderer == null || rollerTransform == null)
            {
                return;
            }
            rollerTransform.localPosition =
                new Vector3(-0, 0, (isFront ? 1.0f : -1.0f) * (_length - _diameter) / 2.0f);
            rollerTransform.localScale = new Vector3(_diameter, (_rollerWidthMultiplier * (_width / 2.0f)), _diameter);

            rollerRenderer.material = _materialRoller;
        }

        private void FixedUpdatePhysicsAndRendering()
        {
            if (!_doRun)
            {
                return;
            }

            _deltaVelocityTime = _velocityMultiplier * Time.deltaTime;

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
            _beltRendererAboveUp.material.mainTextureOffset = _beltMaterialOffset;
            _beltRendererAboveRight.material.mainTextureOffset = _beltMaterialOffset;
            _beltRendererAboveLeft.material.mainTextureOffset = _beltMaterialOffset;

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
                _beltRendererBelowUp.material.mainTextureOffset = _beltMaterialOffset;
                _beltRendererBelowRight.material.mainTextureOffset = _beltMaterialOffset;
                _beltRendererBelowLeft.material.mainTextureOffset = _beltMaterialOffset;
            }
        }

        private void OnValidate()
        {
            CalculateVariables();
            UpdateState();
        }
    }
}