using System.Collections.Generic;
using Zenject;

public class MainMenuStateMachineInstaller : Installer<MainMenuStateMachineInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<TitleScreenStateUI>().AsSingle();
        Container.Bind<MainMenuStateUI>().AsSingle();
        Container.Bind<SettingStateUI>().AsSingle();
        Container.Bind<MainMenuStateMachine>().AsSingle()
                .OnInstantiated<MainMenuStateMachine>((ctx, stateMachine) =>
                {
                    IMainMenuState title = ctx.Container.Resolve<TitleScreenStateUI>();
                    IMainMenuState mainMenu = ctx.Container.Resolve<MainMenuStateUI>();
                    IMainMenuState settings = ctx.Container.Resolve<SettingStateUI>();
                    
                    List<IMainMenuState> states = new List<IMainMenuState> { title, mainMenu, settings };

                    stateMachine.SetStates(states);
                });
    }
}
