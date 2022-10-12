using Composition;
using Controllers;
using Zenject;

namespace Installers
{
    public abstract class ControllersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Initer>().AsSingle().NonLazy();
            Install();
        }
        protected abstract void Install();
    }

    public class MainSceneControllersInstaller : ControllersInstaller
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
