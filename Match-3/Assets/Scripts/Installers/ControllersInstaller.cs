using Composition;
using Controllers;
using Zenject;

namespace Installers
{
    public class ControllersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Initer>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<BundleController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CameraController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GridController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InputController>().AsSingle().NonLazy();
        }
    }
}
