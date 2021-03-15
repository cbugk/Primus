using System.Collections;
using System.Collections.Generic;
using Primus.PrefabRental;
using UnityEngine;

namespace Primus.Sample.PrefabRental
{
    public class Consumer : MonoBehaviour
    {
        private int _frameCount;

        private void Awake()
        {
            _frameCount = 0;
        }

        private void Update()
        {
            if (_frameCount > 19)
            {
                return;
            }

            GameObject myNewObject = Retailer.Instance.GetProduct(ProductBrand.Cylinder);
            if (myNewObject != null)
            {
                myNewObject.transform.SetParent(transform);
                myNewObject.transform.localPosition = Vector3.zero;
                myNewObject.transform.Translate(Vector3.one * _frameCount);

                CylinderProduct myNewCylinder = myNewObject.GetComponent<CylinderProduct>();
                if (myNewCylinder != null)
                {
                    if (_frameCount % 2 == 0)
                    {
                        myNewCylinder.IsSpinning = true;
                    }
                }
            }

            _frameCount++;
        }
    }
}