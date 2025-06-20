using System;
using System.Collections.Generic;
using Zenject;

namespace Feature.UIModule.Scripts
{
    public class UIModuleInstaller : Installer<UIModuleInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IUIService>().To<UIService>().AsSingle()
                .WithArguments(new Dictionary<Type, UIConfig> {
                    { typeof(TitleScreenUI), new UIConfig(Address.UI.TitleScreen, 0, UIWindowType.Normal) },
                    { typeof(MainMenuUI), new UIConfig(Address.UI.MainMenu, 1, UIWindowType.Normal) },
                    { typeof(SettingsUI), new UIConfig(Address.UI.Settings, 2, UIWindowType.Normal) },
                    { typeof(CreditsUI), new UIConfig(Address.UI.Credits, 3, UIWindowType.Normal) },
                    { typeof(LoadGameUI), new UIConfig(Address.UI.LoadGame, 4, UIWindowType.Modal) },
                    { typeof(ScreenTransitionUI), new UIConfig(Address.UI.ScreenTransition, 999, UIWindowType.Popup) }
                });
        }
    }
}
