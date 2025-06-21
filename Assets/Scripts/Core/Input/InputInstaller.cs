using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Core.Input
{
    [CreateAssetMenu(menuName = "Configurations/InputModule/" + nameof(InputInstaller), fileName = nameof(InputInstaller) + "_Default", order = 0)]
    internal class InputInstaller : ScriptableObjectInstaller 
    {
        [SerializeField] private InputActionAsset _inputActionAsset;
        
        public override void InstallBindings() 
        {
            Container.Bind<InputActionAsset>()
                .FromInstance(_inputActionAsset)
                .AsSingle();
        
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputDeviceService>().AsSingle();
        }
    }
}
