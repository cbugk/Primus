using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public class ScrollListBeacon : MonoBehaviour
    {
        [SerializeField] private ButtonBeacon _buttonPrefab;
        [NonSerialized] public List<GameObject> ButtonInstances;
        private GameObject _buttonInstanceCache;
        private RectTransform _viewPortTransform;
        private Transform _contentTransform;
        private List<GameObject> _beaconInstances;
        private Vector2 _sizeDeltaCache;



        private void Awake()
        {
            ButtonInstances = new List<GameObject>();
            _beaconInstances = BeaconEditorManager.Instance.BeaconInstances;
            _viewPortTransform = GetComponentInChildren<Mask>().GetComponent<RectTransform>();
            _contentTransform = GetComponentInChildren<ContentSizeFitter>().GetComponent<Transform>();
        }

        public void AddButton(string name)
        {
            _buttonInstanceCache = GameObject.Instantiate(_buttonPrefab.gameObject);

            _buttonInstanceCache.GetComponent<ButtonBeacon>().Text = name;

            _buttonInstanceCache.transform.SetParent(_contentTransform);

            RectTransform buttonTransform = _buttonInstanceCache.GetComponent<RectTransform>();
            _sizeDeltaCache.x = _viewPortTransform.sizeDelta.x;
            _sizeDeltaCache.y = buttonTransform.sizeDelta.y;
            buttonTransform.sizeDelta = _sizeDeltaCache;

            ButtonInstances.Add(_buttonInstanceCache);
        }

        public void ClearAndPopulate()
        {
            ButtonInstances.Clear();

            foreach (ButtonBeacon buttonInstance in _contentTransform.GetComponentsInChildren<ButtonBeacon>())
            {
                GameObject.Destroy(buttonInstance.gameObject);
            }

            for (int index = 0; index < _beaconInstances.Count; index++)
            {
                AddButton(_beaconInstances[index].name);
            }
            _buttonInstanceCache = null;
        }
    }
}
