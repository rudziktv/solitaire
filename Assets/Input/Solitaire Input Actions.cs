//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/Solitaire Input Actions.inputactions
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

public partial class @SolitaireInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @SolitaireInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Solitaire Input Actions"",
    ""maps"": [
        {
            ""name"": ""Standard"",
            ""id"": ""6e56ac03-59a0-4da7-9050-2dbe0d0cb176"",
            ""actions"": [
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""e5139e3e-21c5-4c19-b2aa-59e10a8adc94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""774f5384-b702-4b71-a974-3ffe1a1a5be3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Standard
        m_Standard = asset.FindActionMap("Standard", throwIfNotFound: true);
        m_Standard_Exit = m_Standard.FindAction("Exit", throwIfNotFound: true);
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

    // Standard
    private readonly InputActionMap m_Standard;
    private List<IStandardActions> m_StandardActionsCallbackInterfaces = new List<IStandardActions>();
    private readonly InputAction m_Standard_Exit;
    public struct StandardActions
    {
        private @SolitaireInputActions m_Wrapper;
        public StandardActions(@SolitaireInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Exit => m_Wrapper.m_Standard_Exit;
        public InputActionMap Get() { return m_Wrapper.m_Standard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(StandardActions set) { return set.Get(); }
        public void AddCallbacks(IStandardActions instance)
        {
            if (instance == null || m_Wrapper.m_StandardActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_StandardActionsCallbackInterfaces.Add(instance);
            @Exit.started += instance.OnExit;
            @Exit.performed += instance.OnExit;
            @Exit.canceled += instance.OnExit;
        }

        private void UnregisterCallbacks(IStandardActions instance)
        {
            @Exit.started -= instance.OnExit;
            @Exit.performed -= instance.OnExit;
            @Exit.canceled -= instance.OnExit;
        }

        public void RemoveCallbacks(IStandardActions instance)
        {
            if (m_Wrapper.m_StandardActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IStandardActions instance)
        {
            foreach (var item in m_Wrapper.m_StandardActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_StandardActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public StandardActions @Standard => new StandardActions(this);
    public interface IStandardActions
    {
        void OnExit(InputAction.CallbackContext context);
    }
}