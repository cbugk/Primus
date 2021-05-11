using System.Linq;
using UnityEngine;

namespace Primus.Core.Singleton
{
    public abstract class BaseScriptableSingleton<T>
        : ScriptableObject
        where T : ScriptableObject
    {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                return _instance;
            }
        }

        protected virtual void OnEnable()
        {
            DontDestroyOnLoad(this);
        }
    }
}