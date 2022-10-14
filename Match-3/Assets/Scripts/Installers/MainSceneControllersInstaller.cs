using Composition;
using Controllers;
using Zenject;

namespace Installers
{
    public abstract class InstallersWithInitBase : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInitBehaviour>().To<Initer>().AsSingle().NonLazy();
            Install();
        }
        protected abstract void Install();
    }

    public class MainSceneControllersInstaller : InstallersWithInitBase
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
