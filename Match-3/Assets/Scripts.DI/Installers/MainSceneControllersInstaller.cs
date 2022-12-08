using Controllers;
using DB;
using Scripts.Main;
using Scripts.Main.Controllers;
using Scripts.Main.DB;
using Zenject;

namespace Scripts.DI.Installers
{
    public abstract class ControllersInstallerBase : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInitiator>().To<Initiator>().AsSingle().NonLazy();
            Container.Bind<IDatabaseDispatcher>().To<DatabaseDispatcher>().AsSingle().NonLazy();
            Install();
        }
        protected abstract void Install();
    }

    public class MainSceneControllersInstaller : ControllersInstallerBase
    {
        protected override void Install()
        {
            Container.BindInterfacesAndSelfTo<MainSceneUIController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CameraController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GridController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle().NonLazy();
        }
    }
}
