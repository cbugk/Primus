using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primus.PrefabRental.Sample
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
            if (_frameCount > 20)
            {
                return;
            }

            GameObject myNewObject = Retailer.Instance.GetProduct((int)Product.Cylinder);
            if (myNewObject != null)
            {
                myNewObject.transform.SetParent(transform);
                myNewObject.transform.localPosition = Vector3.zero;
                myNewObject.transform.Translate(Vector3.one * _frameCount);

                CylinderProduct myNewCube = myNewObject.GetComponent<CylinderProduct>();
                if (_frameCount % 2 == 0)
                {
                    myNewCube.IsSpinning = true;
                }
            }

            _frameCount++;
        }
    }
}