using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core.Singleton;
using Primus.Toolbox.Service;

namespace Primus.Toolbox.Input
{
    //[System.Serializable]
    public interface IDaemonInput<TSceneType, TDaemonScene>
        where TSceneType : Enum
        where TDaemonScene : BaseMonoSingleton<TDaemonScene>, IDaemonScene<TSceneType>
    {
    }
}
