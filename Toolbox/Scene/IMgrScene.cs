using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core.Singleton;
using Primus.Toolbox.UI;
using Primus.Toolbox.Input;

namespace Primus.Toolbox.Scene
{
    public interface IMgrScene<TSceneType, TInputActions>
        where TSceneType : Enum
        where TInputActions : IInputActionCollection
    {
        public MgrCursor CursorManager { get; }
        public MgrCamera CameraManager { get; }

        public void InitialLoadNextScene();

        public void OnCameraManagerZoom(InputAction.CallbackContext context);
        public void OnCameraManagerMove(InputAction.CallbackContext context);
    }
}
