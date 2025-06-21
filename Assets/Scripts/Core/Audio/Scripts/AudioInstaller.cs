using AddressablesAddress;
using Core.AssetLoader;
using Core.Audio.Scripts;
using Core.Installer;

public class AudioInstaller : Installer<AudioInstaller>
{
    public override void InstallBindings()
    {
        IAddressablesAssetLoaderService addressablesAssetLoaderService = Container.Resolve<IAddressablesAssetLoaderService>();
        Container.Bind<MusicConfig>()
            .FromScriptableObject(addressablesAssetLoaderService.LoadAsset<MusicConfig>(Address.Configs.MusicConfig))
            .AsSingle();
        Container.Bind<SFXConfig>()
            .FromScriptableObject(addressablesAssetLoaderService.LoadAsset<SFXConfig>(Address.Configs.SFXConfig))
            .AsSingle();

        Container.BindInterfacesAndSelfTo<AudioService>().AsSingle();
    }
}
