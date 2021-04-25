// GENERATED AUTOMATICALLY FROM 'Assets/Primus/Core/PrimusInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Primus.Core
{
    public class @PrimusInput : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PrimusInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PrimusInput"",
    ""maps"": [
        {
            ""name"": ""Beacon"",
            ""id"": ""ba1f2000-9202-4678-a0a0-2418569ee2c1"",
            ""actions"": [
                {
                    ""name"": ""MoveLock"",
                    ""type"": ""Button"",
                    ""id"": ""c84c4964-62d8-4ddd-acf9-985a500339da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UnlockAdd"",
                    ""type"": ""Button"",
                    ""id"": ""8578702b-15cb-41a7-a68f-1021b6f8b5d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Add"",
                    ""type"": ""Button"",
                    ""id"": ""871ff851-40be-48a8-a21b-ae02a8ffdec9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""0be6a015-b72a-4b3b-bde0-22d7cd0f2a74"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""0f79415c-77df-43d7-a49e-98b6fcb0f5d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5334bdad-c13e-40d9-91f0-54528cd034a4"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""227cff89-f498-4f7e-82d6-1c3162f7997b"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockAdd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ebc840c9-b70f-49aa-913c-b9b50b4097f9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Add"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bfe272b6-2eb4-4e96-a8c5-436961493378"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6642451d-24e9-4a6b-a498-aac9f9f9530e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36981cfc-e26d-471f-bb8f-4861fbf08e73"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CameraManager"",
            ""id"": ""cc417fe5-0af7-4bb5-b28c-7f674611776a"",
            ""actions"": [
                {
                    ""name"": ""SwitchToModCam"",
                    ""type"": ""Value"",
                    ""id"": ""b3ce9ecb-3c00-43db-82bd-c923e2728975"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""ce5cf1fa-0323-48f0-8565-14ec0fb6d027"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""3c758d20-132c-48dc-9641-d8fd38e0fddd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""InvertVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveLock"",
                    ""type"": ""Button"",
                    ""id"": ""bf289342-39db-4cb6-8d9a-ba8952d7849e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
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
                    ""path"": ""<Keyboard>/OEM1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchToModCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4017370a-e5d5-4475-b2d1-e0ebdc68e7e1"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c365a32b-b5c6-41cc-af04-922065d883eb"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""679d533f-4cba-4f17-a548-138d538e3291"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CanvasManager"",
            ""id"": ""56a4a0d0-e3da-4307-a36f-c5ce8f1a4ea0"",
            ""actions"": [
                {
                    ""name"": ""ToggleEscMenu"",
                    ""type"": ""Button"",
                    ""id"": ""935ec9b1-d3c8-4766-9cda-942ab445d3b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UpdatePanelBeaconSelection"",
                    ""type"": ""Button"",
                    ""id"": ""20d1c796-d4f1-4b9f-8410-b5357267caa5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1900c65b-6ae9-422b-aaf6-bd4e34dd0e65"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleEscMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""881e9971-34b3-4365-8e3e-41559ef85e1d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpdatePanelBeaconSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BeaconEditorManager"",
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
            // Beacon
            m_Beacon = asset.FindActionMap("Beacon", throwIfNotFound: true);
            m_Beacon_MoveLock = m_Beacon.FindAction("MoveLock", throwIfNotFound: true);
            m_Beacon_UnlockAdd = m_Beacon.FindAction("UnlockAdd", throwIfNotFound: true);
            m_Beacon_Add = m_Beacon.FindAction("Add", throwIfNotFound: true);
            m_Beacon_Select = m_Beacon.FindAction("Select", throwIfNotFound: true);
            m_Beacon_Delete = m_Beacon.FindAction("Delete", throwIfNotFound: true);
            // CameraManager
            m_CameraManager = asset.FindActionMap("CameraManager", throwIfNotFound: true);
            m_CameraManager_SwitchToModCam = m_CameraManager.FindAction("SwitchToModCam", throwIfNotFound: true);
            m_CameraManager_Zoom = m_CameraManager.FindAction("Zoom", throwIfNotFound: true);
            m_CameraManager_Move = m_CameraManager.FindAction("Move", throwIfNotFound: true);
            m_CameraManager_MoveLock = m_CameraManager.FindAction("MoveLock", throwIfNotFound: true);
            // CanvasManager
            m_CanvasManager = asset.FindActionMap("CanvasManager", throwIfNotFound: true);
            m_CanvasManager_ToggleEscMenu = m_CanvasManager.FindAction("ToggleEscMenu", throwIfNotFound: true);
            m_CanvasManager_UpdatePanelBeaconSelection = m_CanvasManager.FindAction("UpdatePanelBeaconSelection", throwIfNotFound: true);
            // BeaconEditorManager
            m_BeaconEditorManager = asset.FindActionMap("BeaconEditorManager", throwIfNotFound: true);
            m_BeaconEditorManager_ToggleActive = m_BeaconEditorManager.FindAction("ToggleActive", throwIfNotFound: true);
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

        // Beacon
        private readonly InputActionMap m_Beacon;
        private IBeaconActions m_BeaconActionsCallbackInterface;
        private readonly InputAction m_Beacon_MoveLock;
        private readonly InputAction m_Beacon_UnlockAdd;
        private readonly InputAction m_Beacon_Add;
        private readonly InputAction m_Beacon_Select;
        private readonly InputAction m_Beacon_Delete;
        public struct BeaconActions
        {
            private @PrimusInput m_Wrapper;
            public BeaconActions(@PrimusInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveLock => m_Wrapper.m_Beacon_MoveLock;
            public InputAction @UnlockAdd => m_Wrapper.m_Beacon_UnlockAdd;
            public InputAction @Add => m_Wrapper.m_Beacon_Add;
            public InputAction @Select => m_Wrapper.m_Beacon_Select;
            public InputAction @Delete => m_Wrapper.m_Beacon_Delete;
            public InputActionMap Get() { return m_Wrapper.m_Beacon; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(BeaconActions set) { return set.Get(); }
            public void SetCallbacks(IBeaconActions instance)
            {
                if (m_Wrapper.m_BeaconActionsCallbackInterface != null)
                {
                    @MoveLock.started -= m_Wrapper.m_BeaconActionsCallbackInterface.OnMoveLock;
                    @MoveLock.performed -= m_Wrapper.m_BeaconActionsCallbackInterface.OnMoveLock;
                    @MoveLock.canceled -= m_Wrapper.m_BeaconActionsCallbackInterface.OnMoveLock;
                    @UnlockAdd.started -= m_Wrapper.m_BeaconActionsCallbackInterface.OnUnlockAdd;
                    @UnlockAdd.performed -= m_Wrapper.m_BeaconActionsCallbackInterface.OnUnlockAdd;
                    @UnlockAdd.canceled -= m_Wrapper.m_BeaconActionsCallbackInterface.OnUnlockAdd;
                    @Add.started -= m_Wrapper.m_BeaconActionsCallbackInterface.OnAdd;
                    @Add.performed -= m_Wrapper.m_BeaconActionsCallbackInterface.OnAdd;
                    @Add.canceled -= m_Wrapper.m_BeaconActionsCallbackInterface.OnAdd;
                    @Select.started -= m_Wrapper.m_BeaconActionsCallbackInterface.OnSelect;
                    @Select.performed -= m_Wrapper.m_BeaconActionsCallbackInterface.OnSelect;
                    @Select.canceled -= m_Wrapper.m_BeaconActionsCallbackInterface.OnSelect;
                    @Delete.started -= m_Wrapper.m_BeaconActionsCallbackInterface.OnDelete;
                    @Delete.performed -= m_Wrapper.m_BeaconActionsCallbackInterface.OnDelete;
                    @Delete.canceled -= m_Wrapper.m_BeaconActionsCallbackInterface.OnDelete;
                }
                m_Wrapper.m_BeaconActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MoveLock.started += instance.OnMoveLock;
                    @MoveLock.performed += instance.OnMoveLock;
                    @MoveLock.canceled += instance.OnMoveLock;
                    @UnlockAdd.started += instance.OnUnlockAdd;
                    @UnlockAdd.performed += instance.OnUnlockAdd;
                    @UnlockAdd.canceled += instance.OnUnlockAdd;
                    @Add.started += instance.OnAdd;
                    @Add.performed += instance.OnAdd;
                    @Add.canceled += instance.OnAdd;
                    @Select.started += instance.OnSelect;
                    @Select.performed += instance.OnSelect;
                    @Select.canceled += instance.OnSelect;
                    @Delete.started += instance.OnDelete;
                    @Delete.performed += instance.OnDelete;
                    @Delete.canceled += instance.OnDelete;
                }
            }
        }
        public BeaconActions @Beacon => new BeaconActions(this);

        // CameraManager
        private readonly InputActionMap m_CameraManager;
        private ICameraManagerActions m_CameraManagerActionsCallbackInterface;
        private readonly InputAction m_CameraManager_SwitchToModCam;
        private readonly InputAction m_CameraManager_Zoom;
        private readonly InputAction m_CameraManager_Move;
        private readonly InputAction m_CameraManager_MoveLock;
        public struct CameraManagerActions
        {
            private @PrimusInput m_Wrapper;
            public CameraManagerActions(@PrimusInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @SwitchToModCam => m_Wrapper.m_CameraManager_SwitchToModCam;
            public InputAction @Zoom => m_Wrapper.m_CameraManager_Zoom;
            public InputAction @Move => m_Wrapper.m_CameraManager_Move;
            public InputAction @MoveLock => m_Wrapper.m_CameraManager_MoveLock;
            public InputActionMap Get() { return m_Wrapper.m_CameraManager; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CameraManagerActions set) { return set.Get(); }
            public void SetCallbacks(ICameraManagerActions instance)
            {
                if (m_Wrapper.m_CameraManagerActionsCallbackInterface != null)
                {
                    @SwitchToModCam.started -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnSwitchToModCam;
                    @SwitchToModCam.performed -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnSwitchToModCam;
                    @SwitchToModCam.canceled -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnSwitchToModCam;
                    @Zoom.started -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnZoom;
                    @Zoom.performed -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnZoom;
                    @Zoom.canceled -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnZoom;
                    @Move.started -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnMove;
                    @MoveLock.started -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnMoveLock;
                    @MoveLock.performed -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnMoveLock;
                    @MoveLock.canceled -= m_Wrapper.m_CameraManagerActionsCallbackInterface.OnMoveLock;
                }
                m_Wrapper.m_CameraManagerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SwitchToModCam.started += instance.OnSwitchToModCam;
                    @SwitchToModCam.performed += instance.OnSwitchToModCam;
                    @SwitchToModCam.canceled += instance.OnSwitchToModCam;
                    @Zoom.started += instance.OnZoom;
                    @Zoom.performed += instance.OnZoom;
                    @Zoom.canceled += instance.OnZoom;
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @MoveLock.started += instance.OnMoveLock;
                    @MoveLock.performed += instance.OnMoveLock;
                    @MoveLock.canceled += instance.OnMoveLock;
                }
            }
        }
        public CameraManagerActions @CameraManager => new CameraManagerActions(this);

        // CanvasManager
        private readonly InputActionMap m_CanvasManager;
        private ICanvasManagerActions m_CanvasManagerActionsCallbackInterface;
        private readonly InputAction m_CanvasManager_ToggleEscMenu;
        private readonly InputAction m_CanvasManager_UpdatePanelBeaconSelection;
        public struct CanvasManagerActions
        {
            private @PrimusInput m_Wrapper;
            public CanvasManagerActions(@PrimusInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @ToggleEscMenu => m_Wrapper.m_CanvasManager_ToggleEscMenu;
            public InputAction @UpdatePanelBeaconSelection => m_Wrapper.m_CanvasManager_UpdatePanelBeaconSelection;
            public InputActionMap Get() { return m_Wrapper.m_CanvasManager; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CanvasManagerActions set) { return set.Get(); }
            public void SetCallbacks(ICanvasManagerActions instance)
            {
                if (m_Wrapper.m_CanvasManagerActionsCallbackInterface != null)
                {
                    @ToggleEscMenu.started -= m_Wrapper.m_CanvasManagerActionsCallbackInterface.OnToggleEscMenu;
                    @ToggleEscMenu.performed -= m_Wrapper.m_CanvasManagerActionsCallbackInterface.OnToggleEscMenu;
                    @ToggleEscMenu.canceled -= m_Wrapper.m_CanvasManagerActionsCallbackInterface.OnToggleEscMenu;
                    @UpdatePanelBeaconSelection.started -= m_Wrapper.m_CanvasManagerActionsCallbackInterface.OnUpdatePanelBeaconSelection;
                    @UpdatePanelBeaconSelection.performed -= m_Wrapper.m_CanvasManagerActionsCallbackInterface.OnUpdatePanelBeaconSelection;
                    @UpdatePanelBeaconSelection.canceled -= m_Wrapper.m_CanvasManagerActionsCallbackInterface.OnUpdatePanelBeaconSelection;
                }
                m_Wrapper.m_CanvasManagerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ToggleEscMenu.started += instance.OnToggleEscMenu;
                    @ToggleEscMenu.performed += instance.OnToggleEscMenu;
                    @ToggleEscMenu.canceled += instance.OnToggleEscMenu;
                    @UpdatePanelBeaconSelection.started += instance.OnUpdatePanelBeaconSelection;
                    @UpdatePanelBeaconSelection.performed += instance.OnUpdatePanelBeaconSelection;
                    @UpdatePanelBeaconSelection.canceled += instance.OnUpdatePanelBeaconSelection;
                }
            }
        }
        public CanvasManagerActions @CanvasManager => new CanvasManagerActions(this);

        // BeaconEditorManager
        private readonly InputActionMap m_BeaconEditorManager;
        private IBeaconEditorManagerActions m_BeaconEditorManagerActionsCallbackInterface;
        private readonly InputAction m_BeaconEditorManager_ToggleActive;
        public struct BeaconEditorManagerActions
        {
            private @PrimusInput m_Wrapper;
            public BeaconEditorManagerActions(@PrimusInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @ToggleActive => m_Wrapper.m_BeaconEditorManager_ToggleActive;
            public InputActionMap Get() { return m_Wrapper.m_BeaconEditorManager; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(BeaconEditorManagerActions set) { return set.Get(); }
            public void SetCallbacks(IBeaconEditorManagerActions instance)
            {
                if (m_Wrapper.m_BeaconEditorManagerActionsCallbackInterface != null)
                {
                    @ToggleActive.started -= m_Wrapper.m_BeaconEditorManagerActionsCallbackInterface.OnToggleActive;
                    @ToggleActive.performed -= m_Wrapper.m_BeaconEditorManagerActionsCallbackInterface.OnToggleActive;
                    @ToggleActive.canceled -= m_Wrapper.m_BeaconEditorManagerActionsCallbackInterface.OnToggleActive;
                }
                m_Wrapper.m_BeaconEditorManagerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ToggleActive.started += instance.OnToggleActive;
                    @ToggleActive.performed += instance.OnToggleActive;
                    @ToggleActive.canceled += instance.OnToggleActive;
                }
            }
        }
        public BeaconEditorManagerActions @BeaconEditorManager => new BeaconEditorManagerActions(this);
        public interface IBeaconActions
        {
            void OnMoveLock(InputAction.CallbackContext context);
            void OnUnlockAdd(InputAction.CallbackContext context);
            void OnAdd(InputAction.CallbackContext context);
            void OnSelect(InputAction.CallbackContext context);
            void OnDelete(InputAction.CallbackContext context);
        }
        public interface ICameraManagerActions
        {
            void OnSwitchToModCam(InputAction.CallbackContext context);
            void OnZoom(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnMoveLock(InputAction.CallbackContext context);
        }
        public interface ICanvasManagerActions
        {
            void OnToggleEscMenu(InputAction.CallbackContext context);
            void OnUpdatePanelBeaconSelection(InputAction.CallbackContext context);
        }
        public interface IBeaconEditorManagerActions
        {
            void OnToggleActive(InputAction.CallbackContext context);
        }
    }
}
