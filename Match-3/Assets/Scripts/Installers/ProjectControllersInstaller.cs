using Controllers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectControllersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BundleController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneController>().AsSingle().NonLazy();
        }
    }
}
