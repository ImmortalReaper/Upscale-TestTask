using System;
using Core.Input;
using Feature.UIModule.Scripts;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Zenject;

public class TitleScreenUI : BaseUIWindow
{
    private IInputService _inputService;
    private MainMenuStateMachine _mainMenuStateMachine;
    private IDisposable m_EventListener;
    
    [Inject]
    public void InjectDependencies(IInputService inputService, MainMenuStateMachine mainMenuStateMachine)
    {
        _mainMenuStateMachine = mainMenuStateMachine;
        _inputService = inputService;
    }

    private void OnEnable()
    {
        m_EventListener = InputSystem.onAnyButtonPress.Call(HandleSubmit);
    }

    private void OnDisable()
    {
        m_EventListener.Dispose();
    }

    private async void HandleSubmit(InputControl button)
    {
        await _mainMenuStateMachine.ChangeState<MainMenuStateUI>();
    }
}
