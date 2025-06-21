using System;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public interface IInputDeviceService
    {
        public ActiveInputDevice CurrentDevice { get; }
        public InputDevice CurrentInputDevice { get; }
        public event Action<ActiveInputDevice> OnDeviceChanged;
    }
}
