using Core.Installer;

namespace Core.PrefabFactory
{
    public class PrefabFactoryInstaller : Installer<PrefabFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPrefabFactory>().To<PrefabsFactory>().AsSingle();
        }
    }
}
