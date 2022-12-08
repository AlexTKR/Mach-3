using Controllers;
using Scripts.DI.Installers;
using Scripts.Main.Controllers;

namespace Scripts.Main.Installers
{
    public class MapSceneControllersInstaller : ControllersInstallerBase
    {
        protected override void Install()
        {
            Container.BindInterfacesAndSelfTo<MapSceneUIController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MapController>().AsSingle().NonLazy();
        }
    }
}