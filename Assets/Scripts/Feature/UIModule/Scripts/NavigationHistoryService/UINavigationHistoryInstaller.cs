using Core.Installer;

namespace Feature.UIModule.Scripts.NavigationHistoryService
{
    public class UINavigationHistoryInstaller : Installer<UINavigationHistoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UINavigationHistoryService>().AsSingle();
        }
    }
}
