using System;
using UnityEngine.InputSystem;

namespace Primus.Toolbox.Service
{
    public interface IDaemonSceneMain<TSceneType>
        : IDaemonScene<TSceneType>
        where TSceneType : Enum
    {
        /// <summary>Enables or disables instance of manager singleton provided by type.</summary>
        public void SetManagerActive(TSceneType type, bool value);

        /// <summary>Concrete implementation of casting Enum to int.</summary>
        public int IntFromSceneType(TSceneType sceneType);

        /// <summary>Concrete implementation of casting int to Enum.</summary>
        public TSceneType SceneTypeFromInt(int sceneIndex);
    }
}