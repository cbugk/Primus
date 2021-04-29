using UnityEngine;
using UnityEngine.InputSystem;
using Primus.ModTool.Core;

namespace Primus.ModTool.Functionality
{
    [System.Serializable]
    public class MouseToWorldPositionManager : IFunctionality
    {
        [field: SerializeField] public Camera ActiveCamera { get; set; }
        private Ray _ray;
        private RaycastHit _hit;

        private Vector3 _infinity3 = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

        public MouseToWorldPositionManager()
        {
        }

        public (Vector3, GameObject, Vector2, GameObject) GetHitTuple()
        {
            _ray = ActiveCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(_ray, out _hit))
            {
                return (_hit.point, _hit.collider.gameObject, _infinity3, null);
            }

            return (_infinity3, null, _infinity3, null);

        }

        public Vector3 GetCoordinates()
        {
            var result = GetHitTuple();
            if (!result.Item4)
            {
                return GetHitTuple().Item1;
            }
            else
            {
                return _infinity3;
            }
        }
    }
}