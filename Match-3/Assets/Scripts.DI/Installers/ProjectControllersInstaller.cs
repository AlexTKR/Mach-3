using Controllers;
using Scripts.Main.Controllers;
using UnityEngine;
using Zenject;

namespace Scripts.DI.Installers
{
    public class ProjectControllersInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _loadCanvas;

        public override void InstallBindings()
        {
            var loadCanvas =
                Container.InstantiatePrefab(_loadCanvas);
            loadCanvas.SetActive(false);
            
            Container.BindInterfacesAndSelfTo<LoadableController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneController>().AsSingle().WithArguments(loadCanvas).NonLazy(); 
            Container.BindInterfacesAndSelfTo<DatabaseController>().AsSingle().NonLazy();
            Container.Inject(new GameController());
        }
    }
}