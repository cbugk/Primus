using UnityEngine;

namespace Primus.ObjectPool.Example
{
    public class ThingTwo : GenericProduct
    {
        public override void PrepareToUse()
        {
            base.PrepareToUse();
        }

        public void Refurbish()
        {
            ReturnToPool();
        }
    }
}