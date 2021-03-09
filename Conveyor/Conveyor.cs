using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Primus.Conveyor
{
    [ExecuteInEditMode]
    [InitializeOnLoadAttribute]
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

        // belts
        private float _width;
        private float _diameter;
        private float _length;
        private float _tileLength;
        private float _beltThickness;

        private Belt _beltAbove;
        private Belt _beltBelow;

        private BeltRigidbody _beltBodyAbove;
        private BeltRigidbody _beltBodyBelow;
        
        private BeltRenderer _beltRendererAbove;
        private BeltRenderer _beltRendererBelow;

        private BeltTransform _beltTransformAbove;
        private BeltTransform _beltTransformBelow;

        // rollers
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
            EditorApplication.playModeStateChanged += ChildrenDestroyOnExit;

            CalculateVariables();
            CreateParts();
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

        private void CreateParts()
        {
            CreateBelt("BeltAbove", true, out _beltAbove, out _beltBodyAbove, out _beltRendererAbove, out _beltTransformAbove);

            if (_doCreateBeltBelow)
            {
                CreateBelt("BeltBelow", false, out _beltBelow, out _beltBodyBelow, out _beltRendererBelow, out _beltTransformBelow);
            }

            CreateRoller("RollerFront", true, out _rollerBodyFront, out _rollerRendererFront, out _rollerTransformFront);

            CreateRoller("RollerHind", false, out _rollerBodyHind, out _rollerRendererHind, out _rollerTransformHind);
        }

        private void CreateBelt(in string namePrefix, in bool isAbove, out Belt belt, out BeltRigidbody beltBody, out BeltRenderer beltRenderer, out BeltTransform beltTransform)
        {
            // Create outgoing quads.
            belt = new Belt
            {
                IsUpward = isAbove,
                LeftQuad = GameObject.CreatePrimitive(PrimitiveType.Quad),
                UpQuad = GameObject.CreatePrimitive(PrimitiveType.Quad),
                RightQuad = GameObject.CreatePrimitive(PrimitiveType.Quad)
            };

            // Name quads.
            belt.LeftQuad.name = string.Concat(namePrefix, "Left");
            belt.UpQuad.name = string.Concat(namePrefix, "Up");
            belt.RightQuad.name = string.Concat(namePrefix, "Right");

            // Add and set rigidbodies
            beltBody.Left = belt.LeftQuad.AddComponent<Rigidbody>();
            beltBody.Left.isKinematic = true;
            beltBody.Up = belt.UpQuad.AddComponent<Rigidbody>();
            beltBody.Up.isKinematic = true;
            beltBody.Right = belt.RightQuad.AddComponent<Rigidbody>();
            beltBody.Right.isKinematic = true;

            // Set renderers
            beltRenderer.Left = belt.LeftQuad.GetComponent<Renderer>();
            beltRenderer.Up = belt.UpQuad.GetComponent<Renderer>();
            beltRenderer.Right = belt.RightQuad.GetComponent<Renderer>();

            // Set transforms
            beltTransform.Left = belt.LeftQuad.transform;
            beltTransform.Up = belt.UpQuad.transform;
            beltTransform.Right = belt.RightQuad.transform;

            // Set quads as children.
            beltTransform.Left.SetParent(transform);
            beltTransform.Up.SetParent(transform);
            beltTransform.Right.SetParent(transform);
        }

        private void CreateRoller(in string rollerName, in bool isFront, out Rigidbody rollerBody, out Renderer rollerRenderer, out Transform rollerTransform)
        {
            GameObject roller = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            roller.name = rollerName;
            rollerBody = roller.AddComponent<Rigidbody>();
            rollerBody.isKinematic = true;
            rollerRenderer = roller.GetComponent<Renderer>();
            rollerTransform = roller.transform;
            rollerTransform.SetParent(transform);

            rollerTransform.rotation = transform.rotation;

            // Make roller's y direction look transform.right
            roller.transform.localEulerAngles = new Vector3(0, 0, -90);
        }

        private void UpdateState()
        {
            if (_beltAbove.UpQuad != null)
            {
                UpdateBeltState(_beltAbove, _beltRendererAbove, _beltTransformAbove);
            }

            if (_doCreateBeltBelow && _beltBelow.UpQuad != null)
            {
                UpdateBeltState(_beltBelow, _beltRendererBelow, _beltTransformBelow);
            }

            if (_rollerRendererFront != null)
            {
                UpdateRollerState(true, _rollerRendererFront, _rollerTransformFront);
            }

            if (_rollerRendererHind != null)
            {
                UpdateRollerState(false, _rollerRendererHind, _rollerTransformHind);
            }
        }

        private void UpdateBeltState(in Belt belt, BeltRenderer beltRenderer, in BeltTransform beltTransform)
        {
            if (belt.IsUpward)
            {
                beltTransform.Up.localPosition =
                    new Vector3(0.0f, (_diameter / 2.0f), 0.0f);
                beltTransform.Right.localPosition =
                    new Vector3((_width / 2.0f), ((_diameter - _beltThickness) / 2.0f), 0.0f);
                beltTransform.Left.localPosition =
                    new Vector3(-(_width / 2.0f), ((_diameter - _beltThickness) / 2.0f), 0.0f);

                // Norm is conveyor's up
                beltTransform.Up.localRotation = Quaternion.Euler(90, 0, 0);
                // Norm is conveyor's right
                beltTransform.Right.localRotation = Quaternion.Euler(0, -90, -90);
                // Norm is conveyor's left
                beltTransform.Left.localRotation = Quaternion.Euler(0, 90, 90);
            }
            else
            {
                beltTransform.Up.localPosition =
                    new Vector3(0.0f, -(_diameter / 2.0f), 0.0f);
                beltTransform.Right.localPosition =
                    new Vector3((_width / 2.0f), -((_diameter - _beltThickness) / 2.0f), 0.0f);
                beltTransform.Left.localPosition =
                    new Vector3(-(_width / 2.0f), -((_diameter - _beltThickness) / 2.0f), 0.0f);

                // Norm is conveyor's down
                beltTransform.Up.localRotation = Quaternion.Euler(-90, 0, 0);
                // Norm is conveyor's right
                beltTransform.Right.localRotation = Quaternion.Euler(0, -90, 90);
                // Norm is conveyor's left
                beltTransform.Left.localRotation = Quaternion.Euler(0, 90, -90);
            }

            // Set scale (aka size).
            beltTransform.Up.localScale = new Vector3(_width, (_length - _diameter), 1.0f);
            beltTransform.Right.localScale = new Vector3(_beltThickness, (_length - _diameter), 1.0f);
            beltTransform.Left.localScale = beltTransform.Right.localScale;

            // Set materials
            beltRenderer.Up.material = _materialBeltUp;
            beltRenderer.Right.material = _materialBeltSide;
            beltRenderer.Left.material = _materialBeltSide;
        }

        private void UpdateRollerState(in bool isFront, in Renderer rollerRenderer, in Transform rollerTransform)
        {
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
            _beltBodyAbove.Up.position -= _beltStep;
            _beltBodyAbove.Right.position -= _beltStep;
            _beltBodyAbove.Left.position -= _beltStep;
            // Move _beltAbove back (rigidbody)
            _beltBodyAbove.Up.MovePosition(_beltBodyAbove.Up.position + _beltStep);
            _beltBodyAbove.Right.MovePosition(_beltBodyAbove.Right.position + _beltStep);
            _beltBodyAbove.Left.MovePosition(_beltBodyAbove.Left.position + _beltStep);
            // Move material accordingly
            _beltRendererAbove.Up.material.mainTextureOffset = _beltMaterialOffset;
            _beltRendererAbove.Right.material.mainTextureOffset = _beltMaterialOffset;
            _beltRendererAbove.Left.material.mainTextureOffset = _beltMaterialOffset;

            if (_doCreateBeltBelow)
            {
                // Move _beltBelow forward (no physics)
                _beltBodyBelow.Up.position += _beltStep;
                _beltBodyBelow.Right.position += _beltStep;
                _beltBodyBelow.Left.position += _beltStep;
                // Move _beltBelow back (rigidbody)
                _beltBodyBelow.Up.MovePosition(_beltBodyBelow.Up.position - _beltStep);
                _beltBodyBelow.Right.MovePosition(_beltBodyBelow.Right.position - _beltStep);
                _beltBodyBelow.Left.MovePosition(_beltBodyBelow.Left.position - _beltStep);
                // Move tiled material accordingly
                _beltRendererBelow.Up.material.mainTextureOffset = _beltMaterialOffset;
                _beltRendererBelow.Right.material.mainTextureOffset = _beltMaterialOffset;
                _beltRendererBelow.Left.material.mainTextureOffset = _beltMaterialOffset;
            }
        }

        private void OnValidate()
        {
            CalculateVariables();
            UpdateState();
        }

        //private void ChildrenDestroyOnExit(PlayModeStateChange state)
        //{
        //    if (state == PlayModeStateChange.ExitingEditMode) 
        //    {
        //        for (int i = this.transform.childCount; i > 0; --i)
        //            DestroyImmediate(this.transform.GetChild(0).gameObject);
        //    }

        //    if (state == PlayModeStateChange.ExitingPlayMode)
        //    {
        //        for (int i = this.transform.childCount; i > 0; --i)
        //            Destroy(this.transform.GetChild(0).gameObject);
        //    }
        //}

        private void ChildrenDestroyOnExit(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode)
            {
                DestroyImmediate(_beltAbove.LeftQuad, true);
                DestroyImmediate(_beltAbove.UpQuad, true);
                DestroyImmediate(_beltAbove.RightQuad, true);

                DestroyImmediate(_beltBelow.LeftQuad);
                DestroyImmediate(_beltBelow.UpQuad);
                DestroyImmediate(_beltBelow.RightQuad);

                DestroyImmediate(_rollerBodyFront.gameObject);
                DestroyImmediate(_rollerBodyHind.gameObject);
            }
        }
    }
}