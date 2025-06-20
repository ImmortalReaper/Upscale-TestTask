using System.Collections.Generic;
using Zenject;

public class MainMenuStateMachineInstaller : Installer<MainMenuStateMachineInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<TitleScreenStateUI>().AsSingle();
        Container.Bind<MainMenuStateUI>().AsSingle();
        Container.Bind<SettingStateUI>().AsSingle();
        Container.Bind<CreditsStateUI>().AsSingle();
        Container.Bind<LoadGameStateUI>().AsSingle();
        
        Container.Bind<MainMenuStateMachine>().AsSingle()
                .OnInstantiated<MainMenuStateMachine>((ctx, stateMachine) =>
                {
                    IMainMenuState title = ctx.Container.Resolve<TitleScreenStateUI>();
                    IMainMenuState mainMenu = ctx.Container.Resolve<MainMenuStateUI>();
                    IMainMenuState settings = ctx.Container.Resolve<SettingStateUI>();
                    IMainMenuState credits = ctx.Container.Resolve<CreditsStateUI>();
                    IMainMenuState loadGame = ctx.Container.Resolve<LoadGameStateUI>();
                    
                    List<IMainMenuState> states = new List<IMainMenuState> { title, mainMenu, settings, credits, loadGame };

                    stateMachine.SetStates(states);
                });
    }
}
