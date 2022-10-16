using Controllers;
using UnityEngine;
using UtilitiesAndHelpers;
using View;
using Zenject;

namespace Installers
{
    public class ProjectControllersInstaller : MonoInstaller
    {
        [SerializeField] private LoadCanvas _loadCanvas;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BundleController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PanelController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DatabaseController>().AsSingle().NonLazy();
                
            var loadCanvas =
                Container.InstantiatePrefabForComponent<LoadCanvas>(_loadCanvas).SetActiveStatusAndReturn(false);

            Container.Bind<LoadCanvas>().FromInstance(loadCanvas);
        }
    }
}