using System;
using UnityEngine.InputSystem;
using Zenject;

namespace Core.Input
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private readonly InputActionAsset _inputActions;
        private UIInputService _uiInputService;
        
        public IUIInputService UIInputService => _uiInputService;
        public InputService(InputActionAsset inputActionAsset)
        {
            _inputActions = inputActionAsset;
        }

        public void Initialize()
        {
            _inputActions.Enable();
            _uiInputService = new UIInputService(_inputActions);
            _uiInputService.Initialize();
        }

        public void Dispose()
        {
            _inputActions.Disable();
            _uiInputService?.Dispose();
        }
    }
}
