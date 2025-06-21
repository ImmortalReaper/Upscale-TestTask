using Feature.UIModule.Scripts.MainMenuStateMachine.States;
using UnityEngine;
using Zenject;

namespace Feature.UIModule.Scripts.Menus
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        private MainMenuStateMachine.MainMenuStateMachine _mainMenuStateMachine;
    
        [Inject]
        public void InjectDependencies(MainMenuStateMachine.MainMenuStateMachine mainMenuStateMachine)
        {
            _mainMenuStateMachine = mainMenuStateMachine;
        }
    
        private async void Start()
        {
            await _mainMenuStateMachine.ChangeState<TitleScreenStateUI>();
        }
    }
}
