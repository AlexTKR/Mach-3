using Composition;
using Controllers;
using Zenject;

namespace Installers
{
    public class MapSceneControllersInstaller : InstallersWithInitBase
    {
        protected override void Install()
        {
            Container.BindInterfacesAndSelfTo<MapSceneUIController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MapController>().AsSingle().NonLazy();
        }
    }
}