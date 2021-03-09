using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Primus
{
    //[ExecuteInEditMode]
    public class Conveyor : MonoBehaviour
    {
        // Length and diameter are calculated in terms of one another (and tile numbers)
        [SerializeField] bool _isLengthPrimary = true;
        [SerializeField] bool _doCreateBeltBelow = true;
        [SerializeField] float _velocityMultiplier = 1.0f;
        [SerializeField] float _beltToDiameterRatio = 0.1f;
        [SerializeField] private float _rollerWidthMultiplier = 1.0f;
        // Object for sourcing rotation, position and some size information
        [SerializeField] int _numberBeltTiles = 10;
        [SerializeField] int _numberRollerTiles = 1;
        [SerializeField] Material _materialBeltUp;
        [SerializeField] Material _materialBeltSide;
        [SerializeField] Material _materialRoller;

        float _width;
        float _diameter;
        float _length;
        float _tileLength;
        float _beltThickness;

        Rigidbody _beltAboveUpBody;
        Rigidbody _beltAboveRightBody;
        Rigidbody _beltAboveLeftBody;
        Renderer _beltAboveUpRenderer;
        Renderer _beltAboveRightRenderer;
        Renderer _beltAboveLeftRenderer;

        Rigidbody _beltBelowUpBody;
        Rigidbody _beltBelowRightBody;
        Rigidbody _beltBelowLeftBody;
        Renderer _beltBelowUpRenderer;
        Renderer _beltBelowRightRenderer;
        Renderer _beltBelowLeftRenderer;

        Rigidbody _rollerFrontBody;
        Rigidbody _rollerHindBody;

        // Caching members
        float _deltaVelocityTime;
        Vector3 _beltStep;
        Vector3 _rollerEulerAnglePerTile;
        Quaternion _rollerDeltaRotation;
        float _accumulatedOffset;
        Vector2 _beltMaterialOffset;
        

        void Awake()
        {
            // Cylinder wraps same tile twice, adjusting tile number accordingly.
            _numberRollerTiles *= 2;

            if (_numberBeltTiles.Equals(0) || _numberRollerTiles.Equals(0))
            {
                Debug.Log("Tile numbers for Belt and Roller cannot be zero");
                gameObject.SetActive(false);
                return;
            }

            // Remove Cube Mesh
            var _cubeMeshRenderer = GetComponent<MeshRenderer>();
            if (_cubeMeshRenderer != null) Destroy(_cubeMeshRenderer);
            
            
            Vector3 sizeScale = transform.localScale;
            
            // Scale should be Vector3.one to prevent distortion.
            transform.localScale = Vector3.one;

            _width = sizeScale.x;
            if (_isLengthPrimary)
            {
                _length = sizeScale.z;
                _tileLength = _length / _numberBeltTiles;
                _diameter = _tileLength * _numberRollerTiles / Mathf.PI;
            }
            else
            {
                _diameter = sizeScale.y;
                _tileLength = _diameter * Mathf.PI / _numberRollerTiles;
                _length = _tileLength * _numberBeltTiles;
            }

            // After _diameter is decided upon
            _beltThickness = _beltToDiameterRatio * _diameter;


            _rollerEulerAnglePerTile = new Vector3(0, (360.0f / _numberRollerTiles), 0);

            CreateBelt("BeltAbove", true, out _beltAboveUpBody,out _beltAboveUpRenderer, out _beltAboveRightBody,out _beltAboveRightRenderer, out _beltAboveLeftBody, out _beltAboveLeftRenderer);

            if (_doCreateBeltBelow)
            {
                CreateBelt("BeltBelow", false, out _beltBelowUpBody, out _beltBelowUpRenderer, out _beltBelowRightBody, out _beltBelowRightRenderer, out _beltBelowLeftBody, out _beltBelowLeftRenderer);
            }

            CreateRoller("RollerFront", true, out _rollerFrontBody);
            CreateRoller("RollerHind", false, out _rollerHindBody);

            // Initial cache values.
            _accumulatedOffset = 0.0f;
            _beltMaterialOffset = new Vector2(0.0f, 0.0f);
        }

        void FixedUpdate()
        {
            _deltaVelocityTime = _velocityMultiplier * Time.deltaTime;

            _beltStep = transform.forward * _deltaVelocityTime;

            // Material Offset, 0 <= _accumulatedOffset < _tileLength.
            _accumulatedOffset -= _deltaVelocityTime;
            //_accumulatedOffset %= _tileLength;
            _beltMaterialOffset.y = _accumulatedOffset;

            // Move _beltAbove forward (no physics)
            _beltAboveUpBody.position -= _beltStep;
            _beltAboveRightBody.position -= _beltStep;
            _beltAboveLeftBody.position -= _beltStep;
            // Move _beltAbove back (rigidbody)
            _beltAboveUpBody.MovePosition(_beltAboveUpBody.position + _beltStep);
            _beltAboveRightBody.MovePosition(_beltAboveRightBody.position + _beltStep);
            _beltAboveLeftBody.MovePosition(_beltAboveLeftBody.position + _beltStep);
            // Move material accordingly
            _beltAboveUpRenderer.material.mainTextureOffset = _beltMaterialOffset;
            _beltAboveRightRenderer.material.mainTextureOffset = _beltMaterialOffset;
            _beltAboveLeftRenderer.material.mainTextureOffset = _beltMaterialOffset;

            if (_doCreateBeltBelow)
            {
               // Move _beltBelow forward (no physics)
                _beltBelowUpBody.position += _beltStep;
                _beltBelowRightBody.position += _beltStep;
                _beltBelowLeftBody.position += _beltStep;
                // Move _beltBelow back (rigidbody)
                _beltBelowUpBody.MovePosition(_beltBelowUpBody.position - _beltStep);
                _beltBelowRightBody.MovePosition(_beltBelowRightBody.position - _beltStep);
                _beltBelowLeftBody.MovePosition(_beltBelowLeftBody.position - _beltStep);
                // Move material accordingly
                _beltBelowUpRenderer.material.mainTextureOffset = _beltMaterialOffset;
                _beltBelowRightRenderer.material.mainTextureOffset = _beltMaterialOffset;
                _beltBelowLeftRenderer.material.mainTextureOffset = _beltMaterialOffset;
            }

            // Angle per _deltaVelocityTime displacement
            _rollerDeltaRotation = Quaternion.Euler(_rollerEulerAnglePerTile * _deltaVelocityTime);
            // Turn roller front
            _rollerFrontBody.MoveRotation(_rollerFrontBody.rotation * _rollerDeltaRotation);
            // Turn roller hind
            _rollerHindBody.MoveRotation(_rollerHindBody.rotation * _rollerDeltaRotation);
        }

        void CreateBelt(in string namePrefix, in bool isUpwards, out Rigidbody upBody, out Renderer upRenderer, out Rigidbody rightBody, out Renderer rightRenderer, out Rigidbody leftBody, out Renderer leftRenderer)
        {
            // Create outgoing quads.
            GameObject upQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            GameObject rightQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            GameObject leftQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);

            // Cache transforms.
            Transform upTransform = upQuad.transform;
            Transform rightTransform = rightQuad.transform;
            Transform leftTransform = leftQuad.transform;

            // Add and set rigidbodies
            upBody = upQuad.AddComponent<Rigidbody>();
            upBody.isKinematic = true;
            rightBody = rightQuad.AddComponent<Rigidbody>();
            rightBody.isKinematic = true;
            leftBody = leftQuad.AddComponent<Rigidbody>();
            leftBody.isKinematic = true;

            // Name quads.
            upQuad.name = string.Concat(namePrefix, "Up");
            rightQuad.name = string.Concat(namePrefix, "Right");
            leftQuad.name = string.Concat(namePrefix, "Left");

            // Set quads as children.
            upTransform.SetParent(transform);
            rightTransform.SetParent(transform);
            leftTransform.SetParent(transform);

            if (isUpwards)
            {
                upTransform.localPosition =
                    new Vector3(0.0f, (_diameter / 2.0f), 0.0f);
                rightTransform.localPosition =
                    new Vector3((_width / 2.0f), ((_diameter - _beltThickness) / 2.0f), 0.0f);
                leftTransform.localPosition =
                    new Vector3(-(_width / 2.0f), ((_diameter - _beltThickness) / 2.0f), 0.0f);

                // Norm is conveyor's up
                upTransform.localRotation = Quaternion.Euler(90, 0, 0);
                // Norm is conveyor's right
                rightTransform.localRotation = Quaternion.Euler(0, -90, -90);
                // Norm is conveyor's left
                leftTransform.localRotation = Quaternion.Euler(0, 90, 90);
            }
            else
            {
                upTransform.localPosition =
                    new Vector3(0.0f, -(_diameter / 2.0f), 0.0f);
                rightTransform.localPosition =
                    new Vector3((_width / 2.0f), -((_diameter - _beltThickness) / 2.0f), 0.0f);
                leftTransform.localPosition =
                    new Vector3(-(_width / 2.0f), -((_diameter - _beltThickness) / 2.0f), 0.0f);

                // Norm is conveyor's down
                upTransform.localRotation = Quaternion.Euler(-90, 0, 0);
                // Norm is conveyor's right
                rightTransform.localRotation = Quaternion.Euler(0, -90, 90);
                // Norm is conveyor's left
                leftTransform.localRotation = Quaternion.Euler(0, 90, -90);
            }

            // Set scale (aka size).
            upTransform.localScale = new Vector3(_width, (_length - _diameter), 1.0f);
            rightTransform.localScale = new Vector3(_beltThickness, (_length - _diameter), 1.0f);
            leftTransform.localScale = rightTransform.localScale;

            // Set materials
            upRenderer = upQuad.GetComponent<Renderer>();
            upRenderer.material = _materialBeltUp;
            rightRenderer = rightQuad.GetComponent<Renderer>();
            rightRenderer.material = _materialBeltSide;
            leftRenderer = leftQuad.GetComponent<Renderer>();
            leftRenderer.material = _materialBeltSide;
        }

        void CreateRoller(in string name, in bool isFront, out Rigidbody rollerBody)
        {
            GameObject roller = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            roller.name = name;
            roller.transform.SetParent(transform);

            roller.transform.rotation = transform.rotation;
            roller.transform.localPosition =
                new Vector3(-0, 0, (isFront ? 1.0f : -1.0f) * (_length - _diameter) / 2.0f);
            roller.transform.localScale = new Vector3(_diameter, (_rollerWidthMultiplier * (_width / 2.0f)), _diameter);

            roller.AddComponent<Rigidbody>();
            rollerBody = roller.GetComponent<Rigidbody>();
            rollerBody.isKinematic = true;

            Renderer rollerRenderer = roller.GetComponent<Renderer>();
            rollerRenderer.material = _materialRoller;

            // Make roller's y direction look transform.right
            roller.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
    }
}