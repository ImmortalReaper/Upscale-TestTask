using System;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class UIInputService : IUIInputService
    {
        private const string UI_ACTION_MAP = "UI";
        private const string NEXT_ACTION = "Next";
        private const string PREVIOUS_ACTION = "Previous";
        private const string NEXT_ALT_ACTION = "NextAlt";
        private const string PREVIOUS_ALT_ACTION = "PreviousAlt";
        private const string SUBMIT_ACTION = "Submit";
        private const string CANCEL_ACTION = "Cancel";
    
        private readonly InputActionAsset _inputActions;
        private InputActionMap _uiActionMap;
    
        private InputAction _NextAction;
        private InputAction _PreviousAction;
        private InputAction _NextAltAction;
        private InputAction _PreviousAltAction;
        private InputAction _SubmitAction;
        private InputAction _CancelAction;
    
        public event Action OnNext;
        public event Action OnPrevious;
        public event Action OnNextAlt;
        public event Action OnPreviousAlt;
        public event Action OnSubmit;
        public event Action OnCancel;
    
        public UIInputService(InputActionAsset inputActionAsset)
        {
            _inputActions = inputActionAsset;
        }
    
        public void Initialize()
        {
            _uiActionMap = _inputActions.FindActionMap(UI_ACTION_MAP);
            _NextAction = _uiActionMap.FindAction(NEXT_ACTION);
            _PreviousAction = _uiActionMap.FindAction(PREVIOUS_ACTION);
            _NextAltAction = _uiActionMap.FindAction(NEXT_ALT_ACTION);
            _PreviousAltAction = _uiActionMap.FindAction(PREVIOUS_ALT_ACTION);
            _SubmitAction = _uiActionMap.FindAction(SUBMIT_ACTION);
            _CancelAction = _uiActionMap.FindAction(CANCEL_ACTION);
        
            _uiActionMap.Enable();
        
            _NextAction.performed += OnNextPerformed;
            _PreviousAction.performed += OnPreviousPerformed;
            _NextAltAction.performed += OnNextAltPerformed;
            _PreviousAltAction.performed += OnPreviousAltPerformed;
            _SubmitAction.performed += OnSubmitPerformed;
            _CancelAction.performed += OnCancelPerformed;
        }
    
        public void Dispose()
        {
            _uiActionMap.Disable();
        
            _NextAction.performed -= OnNextPerformed;
            _PreviousAction.performed -= OnPreviousPerformed;
            _NextAltAction.performed -= OnNextAltPerformed;
            _PreviousAltAction.performed -= OnPreviousAltPerformed;
            _SubmitAction.performed -= OnSubmitPerformed;
            _CancelAction.performed -= OnCancelPerformed;
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnCancel?.Invoke();
        }

        private void OnSubmitPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnSubmit?.Invoke();
        }

        private void OnNextPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnNext?.Invoke();
        }

        private void OnPreviousPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnPrevious?.Invoke();
        }

        private void OnNextAltPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnNextAlt?.Invoke();
        }

        private void OnPreviousAltPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnPreviousAlt?.Invoke();
        }

        public void Enable() => _uiActionMap.Enable();
        public void Disable() => _uiActionMap.Disable();
    }
}
