using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primus.Core.Singleton
{
    public interface IMonoSingleton<T>
        where T : MonoBehaviour
    {
        public T Instance { get; }
    }
}
