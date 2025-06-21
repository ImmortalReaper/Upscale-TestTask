using Core.Input;
using Feature.AnimationModule.Scripts;
using Feature.UIModule.Scripts.BacktraceService;
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
            _uiBacktraceService.Back();
        }
    }
}
