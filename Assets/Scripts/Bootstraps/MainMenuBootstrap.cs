using Feature.UIModule.Scripts;
using Feature.UIModule.Scripts.BacktraceService;
using Feature.UIModule.Scripts.MainMenuStateMachine;
using UnityEngine;
using Zenject;

namespace Bootstraps
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