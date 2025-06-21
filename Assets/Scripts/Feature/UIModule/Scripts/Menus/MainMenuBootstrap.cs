using System;
using UnityEngine;
using Zenject;

public class MainMenuBootstrap : MonoBehaviour
{
    private MainMenuStateMachine _mainMenuStateMachine;
    private IInputDeviceService _inputDeviceService;
    
    [Inject]
    public void InjectDependencies(MainMenuStateMachine mainMenuStateMachine, IInputDeviceService inputDeviceService)
    {
        _inputDeviceService = inputDeviceService;
        _mainMenuStateMachine = mainMenuStateMachine;
    }
    
    private async void Start()
    {
        await _mainMenuStateMachine.ChangeState<TitleScreenStateUI>();
        _inputDeviceService.OnDeviceChanged += HandleInputDeviceChanged;
        Debug.Log(_inputDeviceService.CurrentDevice);
    }

    private void OnDestroy()
    {
        _inputDeviceService.OnDeviceChanged -= HandleInputDeviceChanged;
    }

    private void HandleInputDeviceChanged(ActiveInputDevice obj)
    {
        Debug.Log(_inputDeviceService.CurrentDevice);
    }
}
