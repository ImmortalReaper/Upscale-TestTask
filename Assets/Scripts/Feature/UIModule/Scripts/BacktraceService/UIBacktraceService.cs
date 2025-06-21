using System;
using System.Collections.Generic;
using Feature.UIModule.Scripts.MainMenuStateMachine;
using Zenject;

namespace Feature.UIModule.Scripts.BacktraceService
{
    public class UIBacktraceService : IUIBacktraceService, IInitializable, IDisposable
    {
        private MainMenuStateMachine.MainMenuStateMachine _mainMenuStateMachine;
        private Stack<IMainMenuState> _windowBacktrace = new();
        private Stack<IMainMenuState> _modalBacktrace = new();
    
        public bool HasActiveModals => _modalBacktrace.Count > 0;
    
        public UIBacktraceService(MainMenuStateMachine.MainMenuStateMachine mainMenuStateMachine)
        {
            _mainMenuStateMachine = mainMenuStateMachine;
        }
    
        public void Initialize()
        {
            _mainMenuStateMachine.OnStateChanged += HandleWindow;
        }
    
        public void Dispose()
        {
            _mainMenuStateMachine.OnStateChanged -= HandleWindow;
            _windowBacktrace.Clear();
            _modalBacktrace.Clear();
        }
    
        private void HandleWindow(IMainMenuState window)
        {
            switch (window.WindowConfig.WindowType)
            {
                case UIWindowType.Normal:
                    if(window.Window.Backtraced)
                        _windowBacktrace.Push(window);
                    break;
                case UIWindowType.Modal:
                    if (window.Window.Backtraced)
                        _modalBacktrace.Push(window);
                    break;
            }
        }
    
        public async void Back()
        {
            if (_modalBacktrace.Count > 0)
            {
                _modalBacktrace.Pop();
            
                if (_modalBacktrace.Count == 0)
                {
                    var previousWindow = _windowBacktrace.Peek();
                    await _mainMenuStateMachine.ChangeStateWithoutEnter(previousWindow);
                    return;
                }
            
                var previousModal = _modalBacktrace.Peek();
                await _mainMenuStateMachine.ChangeStateWithoutEnter(previousModal);
                return;
            }

            if (_windowBacktrace.Count > 1)
            {
                _windowBacktrace.Pop();
                var previous = _windowBacktrace.Peek();
                await _mainMenuStateMachine.ChangeState(previous);
            }
        }
    
        public void ClearBacktrace() 
        {
            _windowBacktrace.Clear();
            _modalBacktrace.Clear();
        }
    }
}
