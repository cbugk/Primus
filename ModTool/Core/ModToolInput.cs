// GENERATED AUTOMATICALLY FROM 'Assets/Primus/ModTool/Core/ModToolInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Primus.ModTool.Core
{
    public class @ModToolInput : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @ModToolInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""ModToolInput"",
    ""maps"": [
        {
            ""name"": ""PositionSelector"",
            ""id"": ""cc417fe5-0af7-4bb5-b28c-7f674611776a"",
            ""actions"": [
                {
                    ""name"": ""SwitchToModCam"",
                    ""type"": ""Value"",
                    ""id"": ""b3ce9ecb-3c00-43db-82bd-c923e2728975"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Button With Two Modifiers"",
                    ""id"": ""c20f9353-8a3c-43ef-8cd5-ffc5938b304c"",
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
                    ""id"": ""92a46a73-ea88-48e4-8ded-dac6939ace48"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchToModCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""modifier2"",
                    ""id"": ""627cf152-b4c4-4e13-9d7d-c3ec261341f0"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchToModCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""148242ba-ea5f-42a0-ace2-e397a8f28c48"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchToModCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""ModToolManager"",
            ""id"": ""8ea247d8-e084-479d-8b52-7e69acf3346c"",
            ""actions"": [
                {
                    ""name"": ""ToggleActive"",
                    ""type"": ""Button"",
                    ""id"": ""c8e99cb8-e82d-4dde-b15d-e3a8e0f7858b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""3bf55fda-833c-4629-b2ac-fc30d82970ff"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleActive"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""77b5063b-668e-4490-884a-25b1ac6837da"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleActive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""b362ba80-cd14-4867-bf14-f234c846493b"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleActive"",
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
            // ModToolManager
            m_ModToolManager = asset.FindActionMap("ModToolManager", throwIfNotFound: true);
            m_ModToolManager_ToggleActive = m_ModToolManager.FindAction("ToggleActive", throwIfNotFound: true);
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
            private @ModToolInput m_Wrapper;
            public PositionSelectorActions(@ModToolInput wrapper) { m_Wrapper = wrapper; }
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

        // ModToolManager
        private readonly InputActionMap m_ModToolManager;
        private IModToolManagerActions m_ModToolManagerActionsCallbackInterface;
        private readonly InputAction m_ModToolManager_ToggleActive;
        public struct ModToolManagerActions
        {
            private @ModToolInput m_Wrapper;
            public ModToolManagerActions(@ModToolInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @ToggleActive => m_Wrapper.m_ModToolManager_ToggleActive;
            public InputActionMap Get() { return m_Wrapper.m_ModToolManager; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ModToolManagerActions set) { return set.Get(); }
            public void SetCallbacks(IModToolManagerActions instance)
            {
                if (m_Wrapper.m_ModToolManagerActionsCallbackInterface != null)
                {
                    @ToggleActive.started -= m_Wrapper.m_ModToolManagerActionsCallbackInterface.OnToggleActive;
                    @ToggleActive.performed -= m_Wrapper.m_ModToolManagerActionsCallbackInterface.OnToggleActive;
                    @ToggleActive.canceled -= m_Wrapper.m_ModToolManagerActionsCallbackInterface.OnToggleActive;
                }
                m_Wrapper.m_ModToolManagerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ToggleActive.started += instance.OnToggleActive;
                    @ToggleActive.performed += instance.OnToggleActive;
                    @ToggleActive.canceled += instance.OnToggleActive;
                }
            }
        }
        public ModToolManagerActions @ModToolManager => new ModToolManagerActions(this);
        public interface IPositionSelectorActions
        {
            void OnSwitchToModCam(InputAction.CallbackContext context);
        }
        public interface IModToolManagerActions
        {
            void OnToggleActive(InputAction.CallbackContext context);
        }
    }
}
