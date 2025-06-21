using System.Threading.Tasks;
using Feature.UIModule.Scripts.Menus;
using Feature.UIModule.Scripts.ScreenTransition;
using UnityEngine.EventSystems;

namespace Feature.UIModule.Scripts.MainMenuStateMachine.States
{
    public class MainMenuStateUI : IMainMenuState
    {
        private ScreenTransitionUI _screenTransitionUI;
        private IUIService _uiService;
    
        public BaseUIWindow Window => _uiService.TryGet(out MainMenuUI mainMenuUI) ? mainMenuUI : _uiService.Load<MainMenuUI>();
        public UIConfig WindowConfig => _uiService.GetConfig<MainMenuUI>();
    
        public MainMenuStateUI(IUIService uiService)
        {
            _uiService = uiService;
            _screenTransitionUI = _uiService.TryGet(out ScreenTransitionUI screenTransitionUI) ? screenTransitionUI : _uiService.Show<ScreenTransitionUI>();
        }
    
        public Task Enter()
        {
            MainMenuUI mainMenuUI = _uiService.Show<MainMenuUI>();
            EventSystem.current.SetSelectedGameObject(mainMenuUI.FirstSelectable);
            mainMenuUI.PlayMainMenuAnimation();
            _uiService.Unload<TitleScreenUI>();
            _screenTransitionUI.PlayFadeOut();
            return Task.CompletedTask;
        }

        public async Task Exit()
        {
            _screenTransitionUI.PlayFadeIn();
            while (_screenTransitionUI.FadeInPlaying)
                await Task.Yield();
            _uiService.Hide<MainMenuUI>();
        }
    }
}
