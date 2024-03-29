using UnityEngine;
using UnityEngine.InputSystem;
using Primus.ModTool.Core;
using Primus.Core;

namespace Primus.ModTool.Functionality
{
    [System.Serializable]
    public class CursorManager : MonoBehaviour, IFunctionality
    {
        [field: SerializeField] public CursorUIRaycastManager CursorUIRaycastManager { get; private set; }
//        public Camera ActiveCamera { get; set; }
        private Ray _ray;
        private RaycastHit _hit;
        private readonly Vector3 _infinity3 = Vector3.one * Mathf.Infinity;
        private readonly Vector2 _infinity2 = Vector2.one * Mathf.Infinity;

        public void Awake()
        {
            if (CursorUIRaycastManager == null)
            {
                throw new System.MissingMemberException();
            }
        }


        public (Vector3, GameObject, Vector2, GameObject) GetHit(LayerMask layerMask)
        {
            (Vector3, GameObject, Vector2, GameObject) quadruple = (_infinity3, null, _infinity2, null);
            var resultsUI = CursorUIRaycastManager.CursorUIRaycastResults;
            if (resultsUI.Length > 0)
            {
                quadruple.Item3 = resultsUI[0].screenPosition;
                quadruple.Item4 = resultsUI[0].gameObject;
            }

            _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(_ray, out _hit, float.MaxValue, layerMask))
            {
                quadruple.Item1 = _hit.point;
                quadruple.Item2 = _hit.collider.gameObject;
            }

            return quadruple;
        }

        public Vector3 GetCoordinates(LayerMask layerMask)
        {
            var result = GetHit(layerMask);
            if (!result.Item4)
            {
                return result.Item1;
            }
            else
            {
                return _infinity3;
            }
        }
    }
}