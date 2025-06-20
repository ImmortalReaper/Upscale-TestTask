using System.Threading.Tasks;
using Feature.UIModule.Scripts;

public class CreditsStateUI : IMainMenuState
{
    private ScreenTransitionUI _screenTransitionUI;
    private IUIService _uiService;
    
    public BaseUIWindow Window => _uiService.TryGet(out CreditsUI creditsUI) ? creditsUI : _uiService.Load<CreditsUI>();
    public UIConfig WindowConfig => _uiService.GetConfig<CreditsUI>();
    
    public CreditsStateUI(IUIService uiService)
    {
        _uiService = uiService;
        _screenTransitionUI = _uiService.TryGet(out ScreenTransitionUI screenTransitionUI) ? screenTransitionUI : _uiService.Show<ScreenTransitionUI>();
    }
    
    public Task Enter()
    {
        CreditsUI creditsUI = _uiService.Show<CreditsUI>();
        creditsUI.PlayCreditsAnimation();
        _screenTransitionUI.PlayFadeOut();
        return Task.CompletedTask;
    }

    public async Task Exit()
    {
        _screenTransitionUI.PlayFadeIn();
        while (_screenTransitionUI.FadeInPlaying)
            await Task.Yield();
        _uiService.Hide<CreditsUI>();
    }
}
