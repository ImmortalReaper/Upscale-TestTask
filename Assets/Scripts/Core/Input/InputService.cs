using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Object = UnityEngine.Object;

namespace Core.Input
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private readonly InputActionAsset _inputActions;
        private UIInputService _uiInputService;
        private PlayerInput _playerInput;
        
        public PlayerInput PlayerInput => _playerInput;
        public IUIInputService UIInputService => _uiInputService;
        public InputService(InputActionAsset inputActionAsset)
        {
            _inputActions = inputActionAsset;
        }

        public void Initialize()
        {
            CreatePlayerInput();
            _inputActions.Enable();
            _uiInputService = new UIInputService(_inputActions);
            _uiInputService.Initialize();
        }

        public void Dispose()
        {
            _inputActions.Disable();
            _uiInputService?.Dispose();
            Object.Destroy(_playerInput.gameObject);
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
