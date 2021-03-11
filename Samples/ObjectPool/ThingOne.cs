using UnityEngine;

namespace Primus.ObjectPool.Example
{
    public class ThingOne : GenericProduct
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