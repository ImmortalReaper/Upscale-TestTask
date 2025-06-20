using Feature.UIModule.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuUI : BaseUIWindow
{
    [SerializeField] private Button NewGame;
    [SerializeField] private Button LoadGame;
    [SerializeField] private Button Options;
    [SerializeField] private Button Credit;
    [SerializeField] private Button QuitGame;
    [Header("Animations")]
    [SerializeField] private DOTweenSequenceAnimator menuAnimation;
    
    private MainMenuStateMachine _mainMenuStateMachine;
    
    [Inject]
    public void InjectDependencies(MainMenuStateMachine mainMenuStateMachine)
    {
        _mainMenuStateMachine = mainMenuStateMachine;
    }
    
    private void Awake()
    {
        LoadGame.onClick.AddListener(OnLoadGameClicked);
        Options.onClick.AddListener(OnOptionsClicked);
        Credit.onClick.AddListener(OnCreditClicked);
        QuitGame.onClick.AddListener(Application.Quit);
    }

    private void OnDestroy()
    {
        LoadGame.onClick.RemoveListener(OnLoadGameClicked);
        Options.onClick.RemoveListener(OnOptionsClicked);
        Credit.onClick.RemoveListener(OnCreditClicked);
        QuitGame.onClick.RemoveListener(Application.Quit);
    }
    
    public void PlayMainMenuAnimation()
    {
        if (menuAnimation != null)
            menuAnimation.PlaySequence();
    }
    
    private async void OnLoadGameClicked()
    {
        await _mainMenuStateMachine.ChangeStateModal<LoadGameStateUI>();
    }
    
    private async void OnOptionsClicked()
    {
         await _mainMenuStateMachine.ChangeState<SettingStateUI>();
    }

    private async void OnCreditClicked()
    {
        await _mainMenuStateMachine.ChangeState<CreditsStateUI>();
    }
}
