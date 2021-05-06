using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Primus.Core.Singleton;
using Primus.Toolbox.UI;

namespace Primus.Toolbox.Scene
{
    public interface IMgrSceneMain<TSceneType, TInputActions>
        : IMgrScene<TSceneType, TInputActions>
        where TSceneType : Enum
        where TInputActions : IInputActionCollection
    {
        /// <summary>Enables or disables instance of manager singleton provided by type.</summary>
        public void SetManagerActive(TSceneType type, bool value);

        /// <summary>Concrete implementation of casting Enum to int.</summary>
        public int SceneTypeToInt(TSceneType sceneType);

        /// <summary>Concrete implementation of casting int to Enum.</summary>
        public TSceneType IntToSceneType(int sceneIndex);
    }
}