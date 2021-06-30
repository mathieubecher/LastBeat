// GENERATED AUTOMATICALLY FROM 'Assets/Script/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Classic"",
            ""id"": ""22cc3ee5-5ebe-402a-b6b1-f0ab54326e3a"",
            ""actions"": [
                {
                    ""name"": ""Touch"",
                    ""type"": ""Button"",
                    ""id"": ""efc73da5-2f4b-43b5-b6a5-e3693cf8fea4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d70cca32-0c2b-4541-b44e-fac412c8b0f6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Touch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Classic
        m_Classic = asset.FindActionMap("Classic", throwIfNotFound: true);
        m_Classic_Touch = m_Classic.FindAction("Touch", throwIfNotFound: true);
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

    // Classic
    private readonly InputActionMap m_Classic;
    private IClassicActions m_ClassicActionsCallbackInterface;
    private readonly InputAction m_Classic_Touch;
    public struct ClassicActions
    {
        private @InputMaster m_Wrapper;
        public ClassicActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Touch => m_Wrapper.m_Classic_Touch;
        public InputActionMap Get() { return m_Wrapper.m_Classic; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ClassicActions set) { return set.Get(); }
        public void SetCallbacks(IClassicActions instance)
        {
            if (m_Wrapper.m_ClassicActionsCallbackInterface != null)
            {
                @Touch.started -= m_Wrapper.m_ClassicActionsCallbackInterface.OnTouch;
                @Touch.performed -= m_Wrapper.m_ClassicActionsCallbackInterface.OnTouch;
                @Touch.canceled -= m_Wrapper.m_ClassicActionsCallbackInterface.OnTouch;
            }
            m_Wrapper.m_ClassicActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Touch.started += instance.OnTouch;
                @Touch.performed += instance.OnTouch;
                @Touch.canceled += instance.OnTouch;
            }
        }
    }
    public ClassicActions @Classic => new ClassicActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IClassicActions
    {
        void OnTouch(InputAction.CallbackContext context);
    }
}
