using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core.Singleton;
using Primus.Toolbox.View;

namespace Primus.Toolbox.Service
{
    /// <summary>Program's controls API. InputSystem and Canvas callbacks should be directed here.</summary>
    public abstract class BaseBrokerControlsSingleton<TSubclass, TYellowPages, TMngrView, TInputActions>
        : BaseMonoSingleton<TSubclass>
        where TSubclass : MonoBehaviour
        where TYellowPages : BaseYellowPagesSingleton<TYellowPages, TMngrView, TInputActions>
        where TInputActions : IInputActionCollection
        where TMngrView : BaseManagerView
    {
        private TYellowPages _yellowPages;

        private void Start()
        {
            OnSceneChanged();
        }

        private void ValidateFields()
        {
            if (_yellowPages == null) { throw new System.MissingMemberException("YellowPages"); }
        }

        public void OnSceneChanged()
        {
            _yellowPages = FindObjectOfType<TYellowPages>();

            ValidateFields();
        }

        public void OnCameraZoom(InputAction.CallbackContext context)
        {
            // Returns multiples of positive/negative 120.0f
            _yellowPages.ManagerView.ZoomCameraLinear(context.ReadValue<float>());
        }

        public void OnCameraMove(InputAction.CallbackContext context)
        {
            _yellowPages.ManagerView.MoveCamera(context.ReadValue<Vector2>());
        }

        public void OnUnLockCameraMove(bool value)
        {
            _yellowPages.ManagerView.OfficerCamera.CanActiveMove = value;
        }
    }
}