using System.Threading.Tasks;
using Feature.UIModule.Scripts;

public class TitleScreenStateUI : IMainMenuState
{
    private ScreenTransitionUI _screenTransitionUI;
    private IUIService _uiService;
    
    public TitleScreenStateUI(IUIService uiService)
    {
        _uiService = uiService;
    }
    
    public Task Enter()
    {
        _uiService.Show<TitleScreenUI>();
        _uiService.Load<MainMenuUI>();
        _screenTransitionUI = _uiService.Show<ScreenTransitionUI>();
        _screenTransitionUI.PlayFadeOut();
        return Task.CompletedTask;
    }

    public async Task Exit()
    {
        _screenTransitionUI.PlayFadeIn();
        while (_screenTransitionUI.FadeInPlaying)
            await Task.Yield();
        _uiService.Unload<TitleScreenUI>();
    }
}
