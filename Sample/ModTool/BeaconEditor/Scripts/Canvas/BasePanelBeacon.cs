using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Primus.Sample.ModTool.BeaconEditor.Beacon;

namespace Primus.Sample.ModTool.BeaconEditor.Canvas
{
    public abstract class BasePanelBeacon : MonoBehaviour
    {
        [field: NonSerialized] protected BaseBeacon Beacon;
        protected TextMeshProUGUI _textName;
        protected TMP_InputField _fieldName;
        protected TextMeshProUGUI _textRotationAngle;
        protected TMP_InputField _fieldRotationAngle;
        protected Slider _sliderRotationAngle;
        protected float _parsedFloatCache;

        protected virtual void Awake()
        {
            var texts = GetComponentsInChildren<TextMeshProUGUI>();
            var fields = GetComponentsInChildren<TMP_InputField>();
            _textName = texts[0];
            _fieldName = fields[0];
            _textRotationAngle = texts[1];
            _fieldRotationAngle = fields[1];
            _sliderRotationAngle = GetComponentInChildren<Slider>();

            _sliderRotationAngle.minValue = 0.0f;
            _sliderRotationAngle.maxValue = 360.0f;

            _fieldName.onValueChanged.AddListener(OnValueChangedFieldName);
            _fieldRotationAngle.onValueChanged.AddListener(OnValueChangedFieldRotationAngle);
            _sliderRotationAngle.onValueChanged.AddListener(OnValueChangedSliderRotationAngle);
        }

        public virtual void OnValueChangedSliderRotationAngle(float value)
        {
            Beacon.RotationAngle = value;
            UpdatePanelRotation();
        }

        public virtual void OnValueChangedFieldRotationAngle(string value)
        {

            float.TryParse(value, System.Globalization.NumberStyles.AllowDecimalPoint, null, out _parsedFloatCache);
            Beacon.RotationAngle = _parsedFloatCache;

            UpdatePanelRotation();
        }

        public virtual void OnValueChangedFieldName(string value)
        {
            Beacon.name = value;
        }

        internal virtual void ClearBeacon()
        {
            Beacon = null;
        }

        internal virtual void UpdatePanelRotation()
        {
            _fieldRotationAngle.text = Beacon.RotationAngle.ToString();
            _sliderRotationAngle.value = Beacon.RotationAngle;
        }

        internal virtual void UpdateFieldName()
        {
            _textName.text = Beacon.name;
        }
    }
}
