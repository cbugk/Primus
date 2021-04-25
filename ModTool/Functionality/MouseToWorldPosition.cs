using UnityEngine;
using UnityEngine.InputSystem;
using Primus.ModTool.Core;

namespace Primus.ModTool.Functionality
{
    [System.Serializable]
    public class MouseToWorldPosition : IFunctionality
    {
        [field: SerializeField] public Camera ActiveCamera { get; set; }
        private Ray _ray;
        private RaycastHit _hit;

        private Vector3 _infinity3 = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);

        public MouseToWorldPosition()
        {
        }

        public (Vector3, GameObject) GetHitTuple()
        {
            _ray = ActiveCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(_ray, out _hit))
            {
                return (_hit.point, _hit.collider.gameObject);
            }

            return (_infinity3, null);
        }

        public Vector3 GetCoordinates()
        {
            return GetHitTuple().Item1;
        }
    }
}