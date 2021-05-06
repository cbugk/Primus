using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core.Singleton;
using Primus.Toolbox.Scene;

namespace Primus.Toolbox.Input
{
    //[System.Serializable]
    public interface IMgrInput<TSceneType, TInputActions, TMgrScene>
        where TSceneType : Enum
        where TInputActions : IInputActionCollection
        where TMgrScene : BaseMonoSingleton<TMgrScene>, IMgrScene<TSceneType, TInputActions>
    {
        public TInputActions InputActions { get; }
    }
}
