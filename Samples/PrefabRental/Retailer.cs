using Primus.PrefabRental;
using UnityEngine;

namespace Primus.Sample.PrefabRental
{
    public class Retailer : GenericRetailer<ProductBrand>
    {
        [SerializeField] private BaseProduct[] _catalog;

        protected override void Start()
        {
            ProductCatalog = _catalog;
            _catalog = null;
            base.Start();
        }
    }
}