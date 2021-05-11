using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Primus.Core.Singleton;
using Primus.Toolbox.View;
using Primus.Toolbox.Input;

namespace Primus.Toolbox.Service
{
    public interface IDaemonScene<TSceneType>
        where TSceneType : Enum
    {
        public void InitialLoadNextScene();
    }
}
