using Feature.UIModule.Scripts.MenuStateMachine;
using Feature.UIModule.Scripts.MenuStateMachine.States;
using UnityEngine;
using Zenject;

namespace Feature.UIModule.Scripts.Menus
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        private MainMenuStateMachine _mainMenuStateMachine;
    
        [Inject]
        public void InjectDependencies(MainMenuStateMachine mainMenuStateMachine)
        {
            _mainMenuStateMachine = mainMenuStateMachine;
        }
    
        private async void Start()
        {
            await _mainMenuStateMachine.ChangeState<TitleScreenStateUI>();
        }
    }
}
