using Core.Installer;

public class UIBacktraceInstaller : Installer<UIBacktraceInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<UIBacktraceService>().AsSingle();
    }
}
