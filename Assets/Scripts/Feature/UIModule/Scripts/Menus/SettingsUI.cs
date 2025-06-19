using Core.Input;
using Feature.UIModule.Scripts;
using Zenject;

public class SettingsUI : BaseUIWindow
{
    private IInputService _inputService;
    private IUIService _uiService;
    
    [Inject]
    public void InjectDependencies(IInputService inputService, IUIService uiService)
    {
        _inputService = inputService;
        _uiService = uiService;
    }

    private void OnEnable()
    {
        _inputService.UIInputService.OnCancel += OnBackPressed;
    }
    
    private void OnDisable()
    {
        _inputService.UIInputService.OnCancel -= OnBackPressed;
    }

    private void OnBackPressed()
    {
        _uiService.Back();
    }
}
