using System.Threading.Tasks;
using Core.Audio.Scripts;
using Feature.UIModule.Scripts.Menus;
using Feature.UIModule.Scripts.ScreenTransition;

namespace Feature.UIModule.Scripts.MainMenuStateMachine.States
{
    public class TitleScreenStateUI : IMainMenuState
    {
        private ScreenTransitionUI _screenTransitionUI;
        private IUIService _uiService;
        private IAudioService _audioService;
    
        public BaseUIWindow Window => _uiService.TryGet(out TitleScreenUI titleScreenUI) ? titleScreenUI : _uiService.Load<TitleScreenUI>();
        public UIConfig WindowConfig => _uiService.GetConfig<TitleScreenUI>();
    
        public TitleScreenStateUI(IUIService uiService, IAudioService audioService)
        {
            _audioService = audioService;
            _uiService = uiService;
        }
    
        public Task Enter()
        {
            _uiService.Show<TitleScreenUI>();
            _uiService.Load<MainMenuUI>();
            _audioService.SetBackgroundMusic(_audioService.MusicConfig.backgroundMusic);
            _screenTransitionUI = _uiService.Show<ScreenTransitionUI>();
            _screenTransitionUI.PlayFadeOut();
            return Task.CompletedTask;
        }

        public async Task Exit()
        {
            _screenTransitionUI.PlayFadeIn();
            while (_screenTransitionUI.FadeInPlaying)
                await Task.Yield();
            _uiService.Hide<TitleScreenUI>();
        }
    }
}
