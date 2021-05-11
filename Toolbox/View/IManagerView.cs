using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Primus.Toolbox.View
{
    public interface IManagerView
    {
        public OfficerCursor OfcCursor { get; }
        public OfficerCamera OfcCamera { get; }
        public void OnCameraManagerZoom(InputAction.CallbackContext context);
        public void OnCameraManagerMove(InputAction.CallbackContext context);
    }
}
