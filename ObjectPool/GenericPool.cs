using System.Collections.Generic;
using UnityEngine;

namespace Primus.ObjectPool
{ 
    public class GenericPool : MonoBehaviour, IPool
    {
        public void ReturnToPool(object instance)
        {
            throw new System.NotImplementedException();
        }
    }

    public class GenericPool<T> : MonoBehaviour, IPool<T> where T : MonoBehaviour, IProduct
    {
        // Reference to prefab.
        [SerializeField] public T Prefab;

        // References to reusable instances
        private Stack<T> _reusableInstances = new Stack<T>();

        /// <returns>Instance of prefab.</returns>
        public T GetPrefabInstance()
        {
            T instance;
            // if we have object in our pool we can use them
            if (_reusableInstances.Count > 0)
            {
                // get object from pool
                instance = _reusableInstances.Pop();

                // remove parent
                instance.transform.SetParent(null);

                // reset position
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localScale = Vector3.one;
                instance.transform.localEulerAngles = Vector3.one;

                // activate object
                instance.gameObject.SetActive(true);
            }
            // otherwise create new instance of prefab
            else
            {
                instance = Instantiate(Prefab);
            }

            // set reference to pool
            instance.Origin = this;
            // and prepare instance for use
            instance.PrepareToUse();

            return instance;
        }

        /// <summary>
        /// Returns instance to the pool.
        /// </summary>
        /// <param name="instance">Prefab instance.</param>
        public void ReturnToPool(T instance)
        {
            // disable object
            instance.gameObject.SetActive(false);

            // set parent as this object
            instance.transform.SetParent(transform);

            // reset position
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = Vector3.one;
            instance.transform.localEulerAngles = Vector3.one;

            // add to pool
            _reusableInstances.Push(instance);
        }

        public void ReturnToPool(object instance)
        {
            // if instance is of our generic type we can return it to our pool
            T product = (T) instance;
            if (product != null) ReturnToPool(product);
        }
    }
}