using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Core.Input
{
    public interface IInputService
    {
        public IUIInputService UIInputService { get; }
        public ReadOnlyArray<InputDevice> Devices { get; }
        public string CurrentControlScheme { get; }
        public event Action<InputDevice, InputDeviceChange> OnDeviceChanged;
    }
}
