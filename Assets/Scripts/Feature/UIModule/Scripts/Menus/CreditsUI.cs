using Core.Input;
using Feature.UIModule.Scripts;
using UnityEngine;
using Zenject;

public class CreditsUI : BaseUIWindow
{
    [Header("Animations")]
    [SerializeField] private DOTweenSequenceAnimator creditsAnimation;
    
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

    public void PlayCreditsAnimation()
    {
        if (creditsAnimation != null)
            creditsAnimation.PlaySequence();
    }
}
