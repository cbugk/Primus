using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Primus.Toolbox.View
{
    public class BaseManagerView : MonoBehaviour
    {
        public OfficerCamera OfficerCamera { get; protected set; }
        public OfficerCursor OfficerCursor { get; protected set; }

        [field: SerializeField] public float ContextQuant { get; protected set; }
        [field: SerializeField] public float MultiplierZoomExponential { get; protected set; }
        [field: SerializeField] public float ClampMinCameraOrthographic { get; protected set; }
        [field: SerializeField] public float ClampMaxCameraOrthographic { get; protected set; }
        [field: SerializeField] public float ClampMinCameraPerspective { get; protected set; }
        [field: SerializeField] public float ClampMaxCameraPerspective { get; protected set; }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            OnSceneActivated();
            ValidateFields();
        }

        protected virtual void OnSceneActivated()
        {
            OfficerCamera = GetComponentInChildren<OfficerCamera>();
            OfficerCursor = GetComponentInChildren<OfficerCursor>();

            OfficerCursor.OfficerCamera = OfficerCamera;
        }

        protected virtual void ValidateFields()
        {
            if (OfficerCamera == null) { throw new System.MissingMemberException($"{name}: OfficerCamera"); }
            if (OfficerCursor == null) { throw new System.MissingMemberException("CursorManager"); }
        }

        public virtual void MoveCamera(Vector2 delta)
        {
            Vector2 moveCache = delta * OfficerCamera.MoveMultiplier;
            OfficerCamera.Move(moveCache.x, moveCache.y);
        }

        public virtual void ZoomCameraLinear(float value)
        {
            Camera activeCam = OfficerCamera.ActiveCamera;
            if (activeCam != null && activeCam.orthographic)
            {
                OfficerCamera.ZoomLinearOrthographic(value / ContextQuant, ClampMinCameraOrthographic, ClampMaxCameraOrthographic);
            }
            else
            {
                OfficerCamera.ZoomLinearPerspective(value / ContextQuant, ClampMinCameraPerspective, ClampMaxCameraPerspective);
            }
        }

        public virtual void ZoomCameraExponential(float value)
        {
            // value represents scaleFactor within if statement.
            // Sets scale factor to specified ratio for zoom-in, or inverse of it for zoom-out.
            if (value < 0)
            {
                value = -value;
                value = Mathf.Pow(MultiplierZoomExponential, (value / ContextQuant));
                value = 1 / value;
            }
            else
            {
                value = Mathf.Pow(MultiplierZoomExponential, (value / ContextQuant));
            }

            if (OfficerCamera.ActiveCamera.orthographic)
            {
                OfficerCamera.ZoomExponentialOrthographic(value);
            }
            else
            {
                OfficerCamera.ZoomExponentialPerspective(value);
            }
        }
    }
}