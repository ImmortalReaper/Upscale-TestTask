using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Zenject;
using Object = UnityEngine.Object;

namespace Core.Input
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private readonly InputActionAsset _inputActions;
        private UIInputService _uiInputService;
        private PlayerInput _playerInput;

        public IUIInputService UIInputService => _uiInputService;
        public ReadOnlyArray<InputDevice> Devices => _playerInput.devices;
        public string CurrentControlScheme => _playerInput.currentControlScheme;
        public event Action<InputDevice, InputDeviceChange> OnDeviceChanged;

        public InputService(InputActionAsset inputActionAsset)
        {
            _inputActions = inputActionAsset;
        }

        public void Initialize()
        {
            CreatePlayerInput();
            InputSystem.onDeviceChange += OnDeviceChange;
            _inputActions.Enable();
            _uiInputService = new UIInputService(_inputActions);
            _uiInputService.Initialize();
        }

        public void Dispose()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
            _inputActions.Disable();
            _uiInputService?.Dispose();
            if(_playerInput != null)
                Object.Destroy(_playerInput.gameObject);
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            OnDeviceChanged?.Invoke(device, change);
        }

        private void CreatePlayerInput()
        {
            GameObject playerInputGO = new GameObject("PlayerInput");
            Object.DontDestroyOnLoad(playerInputGO);

            _playerInput = playerInputGO.AddComponent<PlayerInput>();

            _playerInput.actions = _inputActions;
            _playerInput.defaultControlScheme = "Keyboard & Mouse";
            _playerInput.neverAutoSwitchControlSchemes = false;
        }
    }
}
