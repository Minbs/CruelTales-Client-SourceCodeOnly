//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Inputs/InputActionCollection.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputActionCollection : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActionCollection()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActionCollection"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""e9c5113c-357f-49ed-a177-4bd628360833"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""a7f18409-1085-4e08-b06f-5345656d1af5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""2ee7c429-e022-43c6-9b23-8320ef7bf153"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""a1e98839-d255-4dec-b9d0-c446f8424a03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Walk"",
                    ""type"": ""Button"",
                    ""id"": ""96af4166-df18-4e6a-893f-f18ff5a5ce5a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7df622cd-5b9a-4488-a3a4-0ef695702329"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad_SwitchPro"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0afa46a6-c2a7-48f0-83c3-a0e7b6443de1"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Main"",
                    ""id"": ""7c72a6cd-e35a-496e-bae7-1c36b96fe3a4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6d6a004f-a280-4648-9ef9-6b0df28d9fff"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad_SwitchPro"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""06df58ad-aad8-4c3e-ae01-001ed44a2544"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad_SwitchPro"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8f6592d3-e625-4647-842b-7090b0fab3fb"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad_SwitchPro"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1c68c2b6-ff8a-482d-ad51-b8e6f3498967"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad_SwitchPro"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Direction"",
                    ""id"": ""2216d67f-d95d-4d10-bd90-67f4f6ce10a6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9db4cc26-f147-4283-839c-5555ad757196"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c995029d-9157-4274-93c3-31f9adf10570"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e31cfa89-7a47-4939-9b32-3042fef0757c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1e2eb860-7d24-4142-926e-35c1c95dd6d6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ed3a6278-de46-4921-8ebe-9505aed77fd3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3d082076-4247-4e7e-a459-8af5c73070c8"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f42eec3b-f173-4bf9-9470-cc46e13406c9"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5bfe82e6-3807-4e9f-b7c0-aa37b52b2a53"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""91d3f1ed-2d9d-472e-b2d3-df958577dee7"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0493e026-c858-459b-b3a8-f9bc269acf51"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d92fc5b0-4db5-4256-8655-de15b1230305"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""89253d14-900c-42b3-91dc-8f9cdb5cf62e"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8d234e58-00e9-4094-b0fd-ca66781caaa9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f1cc20a-8a95-4d6b-8a4f-6832e5ee93cb"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GUI"",
            ""id"": ""db2c5288-c2ca-4a6b-bc9c-3c8a005026da"",
            ""actions"": [
                {
                    ""name"": ""TestAction"",
                    ""type"": ""Button"",
                    ""id"": ""74b3446a-a32a-48c7-8cf2-7793ff2d60f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""53c3aabc-a61e-4062-980f-49b5c727553b"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TestAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Global"",
            ""id"": ""13c5eddb-3a59-4502-9820-cf4ba9a584c9"",
            ""actions"": [
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""b3e12ea2-8246-4869-99f3-48ace4ba516d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""40898f50-9430-4b02-b27f-ce6456818097"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be022757-96d5-4ba0-9209-66453fd72015"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad_SwitchPro"",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<VirtualMouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad_SwitchPro"",
            ""bindingGroup"": ""Gamepad_SwitchPro"",
            ""devices"": [
                {
                    ""devicePath"": ""<SwitchProControllerHID>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        m_Gameplay_Interaction = m_Gameplay.FindAction("Interaction", throwIfNotFound: true);
        m_Gameplay_Action = m_Gameplay.FindAction("Action", throwIfNotFound: true);
        m_Gameplay_Walk = m_Gameplay.FindAction("Walk", throwIfNotFound: true);
        // GUI
        m_GUI = asset.FindActionMap("GUI", throwIfNotFound: true);
        m_GUI_TestAction = m_GUI.FindAction("TestAction", throwIfNotFound: true);
        // Global
        m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
        m_Global_Escape = m_Global.FindAction("Escape", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Movement;
    private readonly InputAction m_Gameplay_Interaction;
    private readonly InputAction m_Gameplay_Action;
    private readonly InputAction m_Gameplay_Walk;
    public struct GameplayActions
    {
        private @InputActionCollection m_Wrapper;
        public GameplayActions(@InputActionCollection wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputAction @Interaction => m_Wrapper.m_Gameplay_Interaction;
        public InputAction @Action => m_Wrapper.m_Gameplay_Action;
        public InputAction @Walk => m_Wrapper.m_Gameplay_Walk;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Interaction.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteraction;
                @Action.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAction;
                @Walk.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // GUI
    private readonly InputActionMap m_GUI;
    private IGUIActions m_GUIActionsCallbackInterface;
    private readonly InputAction m_GUI_TestAction;
    public struct GUIActions
    {
        private @InputActionCollection m_Wrapper;
        public GUIActions(@InputActionCollection wrapper) { m_Wrapper = wrapper; }
        public InputAction @TestAction => m_Wrapper.m_GUI_TestAction;
        public InputActionMap Get() { return m_Wrapper.m_GUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GUIActions set) { return set.Get(); }
        public void SetCallbacks(IGUIActions instance)
        {
            if (m_Wrapper.m_GUIActionsCallbackInterface != null)
            {
                @TestAction.started -= m_Wrapper.m_GUIActionsCallbackInterface.OnTestAction;
                @TestAction.performed -= m_Wrapper.m_GUIActionsCallbackInterface.OnTestAction;
                @TestAction.canceled -= m_Wrapper.m_GUIActionsCallbackInterface.OnTestAction;
            }
            m_Wrapper.m_GUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TestAction.started += instance.OnTestAction;
                @TestAction.performed += instance.OnTestAction;
                @TestAction.canceled += instance.OnTestAction;
            }
        }
    }
    public GUIActions @GUI => new GUIActions(this);

    // Global
    private readonly InputActionMap m_Global;
    private IGlobalActions m_GlobalActionsCallbackInterface;
    private readonly InputAction m_Global_Escape;
    public struct GlobalActions
    {
        private @InputActionCollection m_Wrapper;
        public GlobalActions(@InputActionCollection wrapper) { m_Wrapper = wrapper; }
        public InputAction @Escape => m_Wrapper.m_Global_Escape;
        public InputActionMap Get() { return m_Wrapper.m_Global; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
        public void SetCallbacks(IGlobalActions instance)
        {
            if (m_Wrapper.m_GlobalActionsCallbackInterface != null)
            {
                @Escape.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnEscape;
            }
            m_Wrapper.m_GlobalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
            }
        }
    }
    public GlobalActions @Global => new GlobalActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_Gamepad_SwitchProSchemeIndex = -1;
    public InputControlScheme Gamepad_SwitchProScheme
    {
        get
        {
            if (m_Gamepad_SwitchProSchemeIndex == -1) m_Gamepad_SwitchProSchemeIndex = asset.FindControlSchemeIndex("Gamepad_SwitchPro");
            return asset.controlSchemes[m_Gamepad_SwitchProSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
        void OnWalk(InputAction.CallbackContext context);
    }
    public interface IGUIActions
    {
        void OnTestAction(InputAction.CallbackContext context);
    }
    public interface IGlobalActions
    {
        void OnEscape(InputAction.CallbackContext context);
    }
}
