using System;
using System.Collections.Generic;
using Zenject;

namespace Feature.UIModule.Scripts
{
    public class UIModuleInstaller : Installer<UIModuleInstaller>
    {
        public override void InstallBindings()
        {
            /*Container.Bind<IUIService>().To<UIService>().AsSingle()
                .WithArguments(new Dictionary<Type, UIConfig> {
                    { typeof(PlayerUI), new UIConfig(Address.UI.PlayerCanvas, 0) },
                    { typeof(TraderUI), new UIConfig(Address.UI.TraderCanvas, 1) }
                });*/
        }
    }
}
