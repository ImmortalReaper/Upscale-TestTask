using System.Threading.Tasks;
using Feature.UIModule.Scripts.Menus;
using Feature.UIModule.Scripts.ScreenTransition;
using UnityEngine.EventSystems;

namespace Feature.UIModule.Scripts.MainMenuStateMachine.States
{
    public class SettingStateUI : IMainMenuState
    {
        private ScreenTransitionUI _screenTransitionUI;
        private IUIService _uiService;
    
        public BaseUIWindow Window => _uiService.TryGet(out SettingsUI settingsUI) ? settingsUI : _uiService.Load<SettingsUI>();
        public UIConfig WindowConfig => _uiService.GetConfig<SettingsUI>();
    
        public SettingStateUI(IUIService uiService)
        {
            _uiService = uiService;
            _screenTransitionUI = _uiService.TryGet(out ScreenTransitionUI screenTransitionUI) ? screenTransitionUI : _uiService.Show<ScreenTransitionUI>();
        }
    
        public Task Enter()
        {
            SettingsUI settingsUI = _uiService.Show<SettingsUI>();
            EventSystem.current.SetSelectedGameObject(settingsUI.FirstSelectable);
            _screenTransitionUI.PlayFadeOut();
            return Task.CompletedTask;
        }

        public async Task Exit()
        {
            _screenTransitionUI.PlayFadeIn();
            while (_screenTransitionUI.FadeInPlaying)
                await Task.Yield();
            _uiService.Hide<SettingsUI>();
        }
    }
}
