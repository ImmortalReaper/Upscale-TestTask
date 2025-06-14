using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Core.Input
{
    public class InputService : IInputService, IInitializable, IDisposable
    {
        private const string PLAYER_ACTION_MAP = "Player";
        private const string TAP_ACTION = "Tap";
        private const string HOLD_ACTION = "Hold";

        private readonly InputActionAsset _inputActions;
        private InputAction _tapAction;
        private InputAction _holdAction;
        private CancellationTokenSource _holdCts;
        
        public bool IsTapPressed => _tapAction.IsPressed();
        public Vector2 TapPosition => GetInputPosition();
        
        public bool IsHoldPressed => _holdAction.IsPressed();
        public Vector2 HoldPosition => GetInputPosition();
        
        public event Action<Vector2> OnTapPerformed;
        public event Action<Vector2> OnHoldStarted;
        public event Action<Vector2> OnHeld;
        public event Action<Vector2> OnHoldCanceled;

        public InputService(InputActionAsset inputActionAsset)
        {
            _inputActions = inputActionAsset;
        }

        public void Initialize()
        {
            _inputActions.Enable();
            var playerActionMap = _inputActions.FindActionMap(PLAYER_ACTION_MAP);
            _tapAction = playerActionMap.FindAction(TAP_ACTION);
            _holdAction = playerActionMap.FindAction(HOLD_ACTION);

            _tapAction.performed += OnTapAction;
            _holdAction.started += OnHoldAction;
            _holdAction.canceled += OnHoldAction;
        }

        public void Dispose()
        {
            _inputActions.Disable();
            _tapAction.performed -= OnTapAction;
            _holdAction.started -= OnHoldAction;
            _holdAction.canceled -= OnHoldAction;
        }

        private void OnTapAction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnTapPerformed?.Invoke(GetInputPosition());
            }
            
        }

        private void OnHoldAction(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnHoldStarted?.Invoke(GetInputPosition());
                StartHoldAction();
            }
            if (context.canceled)
            {
                StopHoldAction();
                OnHoldCanceled?.Invoke(GetInputPosition());
            }
        }

        private void StartHoldAction()
        {
            _holdCts = new CancellationTokenSource();
            var token = _holdCts.Token;
            HoldLoopAsync(token).Forget();
        }

        private void StopHoldAction()
        {
            if (_holdCts != null && !_holdCts.IsCancellationRequested)
            {
                _holdCts.Cancel();
                _holdCts.Dispose();
                _holdCts = null;
            }
        }
        
        private async UniTask HoldLoopAsync(CancellationToken ct)
        {
            while (_holdAction.IsPressed() && !ct.IsCancellationRequested)
            {
                OnHeld?.Invoke(GetInputPosition());
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: ct);
            }
        }
        
        private Vector2 GetInputPosition()
        {
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
                return Mouse.current.position.ReadValue();

            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
                return Touchscreen.current.primaryTouch.position.ReadValue();

            return Vector2.zero;
        }
    }
}
