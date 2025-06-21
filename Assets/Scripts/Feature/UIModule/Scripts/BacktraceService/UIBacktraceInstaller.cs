using Core.Installer;

namespace Feature.UIModule.Scripts.BacktraceService
{
    public class UIBacktraceInstaller : Installer<UIBacktraceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIBacktraceService>().AsSingle();
        }
    }
}
