using Core.Input;
using Feature.AnimationModule.Scripts;
using Feature.UIModule.Scripts.NavigationHistoryService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Feature.UIModule.Scripts.Menus
{
    public class LoadGameUI : BaseUIWindow
    {
        [SerializeField] private Button cancel;
        [Header("Animations")]
        [SerializeField] private DOTweenSequenceAnimator showLoadGameAnimation;
    
        private IInputService _inputService;
        private IUINavigationHistoryService _uiNavigationHistoryService;
    
        [Inject]
        public void InjectDependencies(IInputService inputService, IUINavigationHistoryService uiNavigationHistoryService)
        {
            _inputService = inputService;
            _uiNavigationHistoryService = uiNavigationHistoryService;
        }

        private void OnEnable()
        {
            _inputService.UIInputService.OnCancel += OnBackPressed;
            cancel.onClick.AddListener(OnBackPressed);
        }
    
        private void OnDisable()
        {
            _inputService.UIInputService.OnCancel -= OnBackPressed;
            cancel.onClick.RemoveListener(OnBackPressed);
        }
    
        public void PlayShowLoadGameAnimation(BaseUIWindow window)
        {
            if (showLoadGameAnimation != null)
                showLoadGameAnimation.PlaySequence();
        }
    
        private void OnBackPressed()
        {
            _uiNavigationHistoryService.Back();
        }
    }
}
