// GENERATED AUTOMATICALLY FROM 'Assets/Primus/Mod/ModManager/ModInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Primus.Mod
{
    public class @ModInputActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @ModInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""ModInputActions"",
    ""maps"": [
        {
            ""name"": ""PositionSelector"",
            ""id"": ""cd4b3705-2735-4395-8a73-736289f289a2"",
            ""actions"": [
                {
                    ""name"": ""SwitchToModCam"",
                    ""type"": ""Value"",
                    ""id"": ""96df4157-5fca-48dd-9064-b6990275af76"",
                    ""expectedControlType"": ""Integer"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Button With Two Modifiers"",
                    ""id"": ""6f193653-0f12-4f63-9df2-fffdae7855a9"",
                    ""path"": ""ButtonWithTwoModifiers"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchToModCam"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier1"",
                    ""id"": ""7b717080-f622-484e-b90b-4e6606181d73"",
                    ""path"": ""<Keyboard>/leftMeta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchToModCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""modifier2"",
                    ""id"": ""f169400d-93cc-43e4-a22a-665ebdeb22af"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchToModCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""31161c0b-a323-43f6-8fd9-aca4295b40ec"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchToModCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PositionSelector
            m_PositionSelector = asset.FindActionMap("PositionSelector", throwIfNotFound: true);
            m_PositionSelector_SwitchToModCam = m_PositionSelector.FindAction("SwitchToModCam", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // PositionSelector
        private readonly InputActionMap m_PositionSelector;
        private IPositionSelectorActions m_PositionSelectorActionsCallbackInterface;
        private readonly InputAction m_PositionSelector_SwitchToModCam;
        public struct PositionSelectorActions
        {
            private @ModInputActions m_Wrapper;
            public PositionSelectorActions(@ModInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @SwitchToModCam => m_Wrapper.m_PositionSelector_SwitchToModCam;
            public InputActionMap Get() { return m_Wrapper.m_PositionSelector; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PositionSelectorActions set) { return set.Get(); }
            public void SetCallbacks(IPositionSelectorActions instance)
            {
                if (m_Wrapper.m_PositionSelectorActionsCallbackInterface != null)
                {
                    @SwitchToModCam.started -= m_Wrapper.m_PositionSelectorActionsCallbackInterface.OnSwitchToModCam;
                    @SwitchToModCam.performed -= m_Wrapper.m_PositionSelectorActionsCallbackInterface.OnSwitchToModCam;
                    @SwitchToModCam.canceled -= m_Wrapper.m_PositionSelectorActionsCallbackInterface.OnSwitchToModCam;
                }
                m_Wrapper.m_PositionSelectorActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SwitchToModCam.started += instance.OnSwitchToModCam;
                    @SwitchToModCam.performed += instance.OnSwitchToModCam;
                    @SwitchToModCam.canceled += instance.OnSwitchToModCam;
                }
            }
        }
        public PositionSelectorActions @PositionSelector => new PositionSelectorActions(this);
        public interface IPositionSelectorActions
        {
            void OnSwitchToModCam(InputAction.CallbackContext context);
        }
    }
}
