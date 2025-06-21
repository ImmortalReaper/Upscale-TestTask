using Core.AssetLoader;
using UnityEngine;
using Zenject;

namespace Bootstraps
{
    [CreateAssetMenu(fileName = "ProjectContext", menuName = "Installers/ProjectContext")]
    public class ProjectContext : ScriptableObjectInstaller<ProjectContext>
    {
        public override void InstallBindings()
        {
            AdressablesAssetLoaderInstaller.Install(Container);
        }
    }
}