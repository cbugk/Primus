using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Primus.Toolbox.UI
{
    public class BaseMgrDropdown<TBiblionTitle> : MonoBehaviour where TBiblionTitle : System.Enum
    {
        public int Value { get => _dropdown.value; }
        protected Dropdown _dropdown;
        protected string[] _dropdownOptionStrings;
        protected virtual void Awake()
        {
            _dropdown = GetComponent<Dropdown>();
            if (!_dropdown) throw new Exception($"[{name}]: Dropdown component could not be found.");
            _dropdownOptionStrings = Enum.GetNames(typeof(TBiblionTitle));
            foreach (var optionString in _dropdownOptionStrings)
            {
                _dropdown.options.Add(new Dropdown.OptionData(optionString));
            }
            _dropdown.SetValueWithoutNotify(0); // TBiblionTitle.DEFAULT
        }

        public void SetValueWithoutNotify(int value) => _dropdown.SetValueWithoutNotify(value);
        public TBiblionTitle GetTitle(int value)
        {
            return (TBiblionTitle)Enum.ToObject(typeof(TBiblionTitle), value);
        }

        public void AddListener(UnityAction<int> action)
        {
            _dropdown.onValueChanged.AddListener(action);
        }

        public void RemoveListener(UnityAction<int> action)
        {
            _dropdown.onValueChanged.RemoveListener(action);
        }
    }
}
