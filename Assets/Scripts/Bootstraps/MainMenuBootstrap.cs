using Feature.UIModule.Scripts;
using Feature.UIModule.Scripts.BacktraceService;
using Feature.UIModule.Scripts.MainMenuStateMachine;
using UnityEngine;
using Zenject;

namespace Bootstraps
{
    [CreateAssetMenu(fileName = "MainMenuBootstrap", menuName = "Installers/MainMenuBootstrap")]
    public class MainMenuBootstrap : ScriptableObjectInstaller<MainMenuBootstrap>
    {
        public override void InstallBindings()
        {
            UIModuleInstaller.Install(Container);
            MainMenuStateMachineInstaller.Install(Container);
            UIBacktraceInstaller.Install(Container);
            AudioInstaller.Install(Container);
        }
    }
}