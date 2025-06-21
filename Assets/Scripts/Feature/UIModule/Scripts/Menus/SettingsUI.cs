using Core.Input;
using Feature.UIModule.Scripts.BacktraceService;
using Zenject;

namespace Feature.UIModule.Scripts.Menus
{
    public class SettingsUI : BaseUIWindow
    {
        private IInputService _inputService;
        private IUIBacktraceService _uiBacktraceService;
    
        [Inject]
        public void InjectDependencies(IInputService inputService, IUIBacktraceService uiBacktraceService)
        {
            _inputService = inputService;
            _uiBacktraceService = uiBacktraceService;
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
            _uiBacktraceService.Back();
        }
    }
}
