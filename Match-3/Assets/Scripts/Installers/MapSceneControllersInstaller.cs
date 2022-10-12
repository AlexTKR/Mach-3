using Composition;
using Controllers;
using Zenject;

namespace Installers
{
    public class MapSceneControllersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Initer>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MapSceneUIController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MapController>().AsSingle().NonLazy();
        }
    }
}
