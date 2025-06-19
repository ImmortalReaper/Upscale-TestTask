using System.Threading.Tasks;
using Feature.UIModule.Scripts;

public class MainMenuStateUI : IMainMenuState
{
    private ScreenTransitionUI _screenTransitionUI;
    private IUIService _uiService;
    
    public MainMenuStateUI(IUIService uiService)
    {
        _uiService = uiService;
        _screenTransitionUI = _uiService.TryGet(out ScreenTransitionUI screenTransitionUI) ? screenTransitionUI : _uiService.Show<ScreenTransitionUI>();
    }
    
    public Task Enter()
    {
        _uiService.Show<MainMenuUI>();
        _screenTransitionUI.PlayFadeOut();
        return Task.CompletedTask;
    }

    public async Task Exit()
    {
        _screenTransitionUI.PlayFadeIn();
        //_uiService.Hide<MainMenuUI>();
        while (_screenTransitionUI.FadeInPlaying)
            await Task.Yield();
    }
}
