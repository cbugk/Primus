// GENERATED AUTOMATICALLY FROM 'Assets/Primus/Samples/ObjectPool/SpawnerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Primus.ObjectPool.Example
{
    public class @SpawnerControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @SpawnerControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""SpawnerControls"",
    ""maps"": [
        {
            ""name"": ""Example"",
            ""id"": ""5ac5b6dc-d57c-4779-8ab4-ad9271024ec2"",
            ""actions"": [
                {
                    ""name"": ""SpawnThingOne"",
                    ""type"": ""Button"",
                    ""id"": ""c0fc4eb4-4a20-4445-8aa7-fa4a3e5161c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpawnThingTwo"",
                    ""type"": ""Button"",
                    ""id"": ""cdfbddc5-e94d-4c0e-b4aa-d59dd21e78cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d6e46b69-91b1-4539-a5cc-fa5411fdb2ae"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnThingOne"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9450beac-0f2d-44e9-b1d0-6c87081770df"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnThingTwo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Example
            m_Example = asset.FindActionMap("Example", throwIfNotFound: true);
            m_Example_SpawnThingOne = m_Example.FindAction("SpawnThingOne", throwIfNotFound: true);
            m_Example_SpawnThingTwo = m_Example.FindAction("SpawnThingTwo", throwIfNotFound: true);
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

        // Example
        private readonly InputActionMap m_Example;
        private IExampleActions m_ExampleActionsCallbackInterface;
        private readonly InputAction m_Example_SpawnThingOne;
        private readonly InputAction m_Example_SpawnThingTwo;
        public struct ExampleActions
        {
            private @SpawnerControls m_Wrapper;
            public ExampleActions(@SpawnerControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @SpawnThingOne => m_Wrapper.m_Example_SpawnThingOne;
            public InputAction @SpawnThingTwo => m_Wrapper.m_Example_SpawnThingTwo;
            public InputActionMap Get() { return m_Wrapper.m_Example; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ExampleActions set) { return set.Get(); }
            public void SetCallbacks(IExampleActions instance)
            {
                if (m_Wrapper.m_ExampleActionsCallbackInterface != null)
                {
                    @SpawnThingOne.started -= m_Wrapper.m_ExampleActionsCallbackInterface.OnSpawnThingOne;
                    @SpawnThingOne.performed -= m_Wrapper.m_ExampleActionsCallbackInterface.OnSpawnThingOne;
                    @SpawnThingOne.canceled -= m_Wrapper.m_ExampleActionsCallbackInterface.OnSpawnThingOne;
                    @SpawnThingTwo.started -= m_Wrapper.m_ExampleActionsCallbackInterface.OnSpawnThingTwo;
                    @SpawnThingTwo.performed -= m_Wrapper.m_ExampleActionsCallbackInterface.OnSpawnThingTwo;
                    @SpawnThingTwo.canceled -= m_Wrapper.m_ExampleActionsCallbackInterface.OnSpawnThingTwo;
                }
                m_Wrapper.m_ExampleActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SpawnThingOne.started += instance.OnSpawnThingOne;
                    @SpawnThingOne.performed += instance.OnSpawnThingOne;
                    @SpawnThingOne.canceled += instance.OnSpawnThingOne;
                    @SpawnThingTwo.started += instance.OnSpawnThingTwo;
                    @SpawnThingTwo.performed += instance.OnSpawnThingTwo;
                    @SpawnThingTwo.canceled += instance.OnSpawnThingTwo;
                }
            }
        }
        public ExampleActions @Example => new ExampleActions(this);
        public interface IExampleActions
        {
            void OnSpawnThingOne(InputAction.CallbackContext context);
            void OnSpawnThingTwo(InputAction.CallbackContext context);
        }
    }
}
