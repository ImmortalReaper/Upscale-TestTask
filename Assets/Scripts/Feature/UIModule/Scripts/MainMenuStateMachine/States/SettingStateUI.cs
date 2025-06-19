using System.Threading.Tasks;
using Feature.UIModule.Scripts;

public class SettingStateUI : IMainMenuState
{
    private ScreenTransitionUI _screenTransitionUI;
    private IUIService _uiService;
    
    public SettingStateUI(IUIService uiService)
    {
        _uiService = uiService;
        _screenTransitionUI = _uiService.TryGet(out ScreenTransitionUI screenTransitionUI) ? screenTransitionUI : _uiService.Show<ScreenTransitionUI>();
    }
    
    public Task Enter()
    {
        _uiService.Show<SettingsUI>();
        _screenTransitionUI.PlayFadeOut();
        return Task.CompletedTask;
    }

    public async Task Exit()
    {
        _screenTransitionUI.PlayFadeIn();
        //_uiService.Hide<SettingsUI>();
        while (_screenTransitionUI.FadeInPlaying)
            await Task.Yield();
    }
}
