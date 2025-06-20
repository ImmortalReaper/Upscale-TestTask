using Feature.UIModule.Scripts;
using UnityEngine;
using Zenject;

namespace ShootingCar.Bootstraps
{
    [CreateAssetMenu(fileName = "GameplayBootstrap", menuName = "Installers/GameplayBootstrap")]
    public class MainMenuBootstrap : ScriptableObjectInstaller<MainMenuBootstrap>
    {
        public override void InstallBindings()
        {
            UIModuleInstaller.Install(Container);
            MainMenuStateMachineInstaller.Install(Container);
            UIBacktraceInstaller.Install(Container);
        }
    }
}