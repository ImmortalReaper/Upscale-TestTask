using System;
using System.Collections.Generic;
using Feature.UIModule.Scripts.MenuStateMachine;
using Zenject;

namespace Feature.UIModule.Scripts.NavigationHistoryService
{
    public class UINavigationHistoryService : IUINavigationHistoryService, IInitializable, IDisposable
    {
        private MainMenuStateMachine _mainMenuStateMachine;
        private Stack<IMainMenuState> _windowHistory = new();
        private Stack<IMainMenuState> _modalHistory = new();
    
        public bool HasActiveModals => _modalHistory.Count > 0;
    
        public UINavigationHistoryService(MainMenuStateMachine mainMenuStateMachine)
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
            _windowHistory.Clear();
            _modalHistory.Clear();
        }
    
        private void HandleWindow(IMainMenuState window)
        {
            switch (window.WindowConfig.WindowType)
            {
                case UIWindowType.Normal:
                    if(window.Window.Backtraced)
                        _windowHistory.Push(window);
                    break;
                case UIWindowType.Modal:
                    if (window.Window.Backtraced)
                        _modalHistory.Push(window);
                    break;
            }
        }
    
        public async void Back()
        {
            if (_modalHistory.Count > 0)
            {
                _modalHistory.Pop();
            
                if (_modalHistory.Count == 0)
                {
                    var previousWindow = _windowHistory.Peek();
                    await _mainMenuStateMachine.ChangeStateWithoutEnter(previousWindow);
                    return;
                }
            
                var previousModal = _modalHistory.Peek();
                await _mainMenuStateMachine.ChangeStateWithoutEnter(previousModal);
                return;
            }

            if (_windowHistory.Count > 1)
            {
                _windowHistory.Pop();
                var previous = _windowHistory.Peek();
                await _mainMenuStateMachine.ChangeState(previous);
            }
        }
    
        public void ClearBacktrace() 
        {
            _windowHistory.Clear();
            _modalHistory.Clear();
        }
    }
}
