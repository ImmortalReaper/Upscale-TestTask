using UnityEngine;
using Zenject;

public class MainMenuBootstrap : MonoBehaviour
{
    private MainMenuStateMachine _mainMenuStateMachine;
    
    [Inject]
    public void InjectDependencies(MainMenuStateMachine mainMenuStateMachine)
    {
        _mainMenuStateMachine = mainMenuStateMachine;
    }

    private void Start() => _mainMenuStateMachine.ChangeState<TitleScreenStateUI>();
}
