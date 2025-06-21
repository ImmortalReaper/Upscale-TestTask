using System.Threading.Tasks;
using Feature.UIModule.Scripts.Menus;
using UnityEngine.EventSystems;

namespace Feature.UIModule.Scripts.MainMenuStateMachine.States
{
    public class LoadGameStateUI : IMainMenuState
    {
        private IUIService _uiService;
    
        public BaseUIWindow Window => _uiService.TryGet(out LoadGameUI loadGameUI) ? loadGameUI : _uiService.Load<LoadGameUI>();
        public UIConfig WindowConfig => _uiService.GetConfig<LoadGameUI>();
    
        public LoadGameStateUI(IUIService uiService)
        {
            _uiService = uiService;
        }
    
        public Task Enter()
        {
            LoadGameUI loadGameUI = _uiService.Show<LoadGameUI>();
            EventSystem.current.SetSelectedGameObject(loadGameUI.FirstSelectable);
            loadGameUI.PlayShowLoadGameAnimation(loadGameUI);
            return Task.CompletedTask;
        }

        public Task Exit()
        {
            _uiService.Hide<LoadGameUI>();
            return Task.CompletedTask;
        }
    }
}
