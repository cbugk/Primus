using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core.Singleton;
using Primus.Toolbox.View;

namespace Primus.Toolbox.Service
{
    public class BaseYellowPagesSingleton<TSubclass, TMngrView, TInputActions>
        : BaseMonoSingleton<TSubclass>
        where TSubclass : MonoBehaviour
        where TMngrView : BaseManagerView
        where TInputActions : IInputActionCollection
    {
        public TMngrView ManagerView { get; protected set; }
        public virtual TInputActions InputActions { get; protected set; }

        protected virtual void Start()
        {
            OnSceneActivated();
        }

        public virtual void OnSceneActivated()
        {
            ManagerView = FindObjectOfType<TMngrView>();

            ValidateFields();
        }

        protected virtual void ValidateFields()
        {
            if (ManagerView == null) { throw new System.MissingMemberException("ManagerView"); }
        }
    }
}
