using UnityEngine;

namespace Primus.Sample.Bibliotheca
{
    public class SamplePatron : MonoBehaviour
    {
        private int _frameCount;

        private void Awake()
        {
            _frameCount = 0;
        }

        private void Update()
        {
            if (_frameCount > 19) return;

            var myNewObject =
                SampleBibliothecaOwnerSingleton.Instance.Bibliotheca.CheckOut(SampleTitleCatalog.Cylinder);
            if (myNewObject != null)
            {
                myNewObject.transform.SetParent(transform);
                myNewObject.transform.localPosition = Vector3.zero;
                myNewObject.transform.Translate(Vector3.one * _frameCount);

                var myNewCylinder = myNewObject.GetComponent<SampleBiblionCylinder>();
                if (myNewCylinder != null)
                    if (_frameCount % 2 == 0)
                        myNewCylinder.IsSpinning = true;
            }

            _frameCount++;
        }
    }
}