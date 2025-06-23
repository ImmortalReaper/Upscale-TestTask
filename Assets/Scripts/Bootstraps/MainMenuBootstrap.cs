using Core.Audio.Scripts;
using Feature.UIModule.Scripts;
using Feature.UIModule.Scripts.MenuStateMachine;
using Feature.UIModule.Scripts.NavigationHistoryService;
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
            UINavigationHistoryInstaller.Install(Container);
            AudioInstaller.Install(Container);
        }
    }
}