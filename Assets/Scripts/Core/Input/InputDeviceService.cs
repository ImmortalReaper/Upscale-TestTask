using System;
using System.Linq;
using Core.Input;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;
using Zenject;

public class InputDeviceService : IInputDeviceService, IInitializable, IDisposable
{
    private readonly IInputService _inputService;
    private ActiveInputDevice _currentDevice = ActiveInputDevice.Unknown;
    private InputDevice _currentInputDevice;
    
    public ActiveInputDevice CurrentDevice => _currentDevice;
    public InputDevice CurrentInputDevice => _currentInputDevice;
    public event Action<ActiveInputDevice> OnDeviceChanged;
    
    public InputDeviceService(IInputService inputService)
    {
        _inputService = inputService;
    }
    
    public void Initialize()
    {
        _inputService.OnDeviceChanged += HandleControlsChanged;
        
        if(_inputService.Devices.Count > 0)
            UpdateDevice(_inputService.Devices.Last());
        else
            UpdateDevice(Keyboard.current ?? (InputDevice)Mouse.current);
    }
    
    public void Dispose()
    {
        _inputService.OnDeviceChanged -= HandleControlsChanged;
    }
    
    private void HandleControlsChanged(InputDevice inputDevice, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
            case InputDeviceChange.Reconnected:
                UpdateDevice(inputDevice);
                break;
            case InputDeviceChange.Removed:
            case InputDeviceChange.Disconnected:
                if (Keyboard.current != null)
                    UpdateDevice(Keyboard.current);
                else if (Mouse.current != null)
                    UpdateDevice(Mouse.current);
                else
                    UpdateDevice(inputDevice);
                break;
        }
    }
    
    private void UpdateDevice(InputDevice device)
    {
        ActiveInputDevice newDevice = ActiveInputDevice.Unknown;
        
        var layout = device.layout.ToLowerInvariant();
        
        if (device is Keyboard || device is Mouse)
            newDevice = ActiveInputDevice.KeyboardAndMouse;
        else if (device is Gamepad)
        {
            if (layout.Contains("dualsense"))
                newDevice = ActiveInputDevice.DualSense;
            else if (layout.Contains("xinput"))
                newDevice = ActiveInputDevice.Xbox;
            else
                newDevice = ActiveInputDevice.Xbox;
        }
        
        if (newDevice != CurrentDevice)
        {
            _currentInputDevice = device;
            _currentDevice = newDevice;
            OnDeviceChanged?.Invoke(newDevice);
        }
    }
}

public enum ActiveInputDevice
{
    KeyboardAndMouse,
    DualSense,
    Xbox,
    Unknown
}
