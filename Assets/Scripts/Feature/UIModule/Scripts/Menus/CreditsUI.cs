using Core.Input;
using Feature.AnimationModule.Scripts;
using Feature.UIModule.Scripts.NavigationHistoryService;
using UnityEngine;
using Zenject;

namespace Feature.UIModule.Scripts.Menus
{
    public class CreditsUI : BaseUIWindow
    {
        [Header("Animations")]
        [SerializeField] private DOTweenSequenceAnimator creditsAnimation;
    
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
        }
    
        private void OnDisable()
        {
            _inputService.UIInputService.OnCancel -= OnBackPressed;
        }

        private void OnBackPressed()
        {
            _uiNavigationHistoryService.Back();
        }

        public void PlayCreditsAnimation()
        {
            if (creditsAnimation != null)
                creditsAnimation.PlaySequence();
        }
    }
}
