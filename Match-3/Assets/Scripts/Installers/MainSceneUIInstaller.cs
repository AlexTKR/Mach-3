using UnityEngine;
using View;
using Zenject;

namespace Installers
{
    public class MainSceneUIInstaller : MonoInstaller
    {
        [SerializeField] private MainCanvas _mainCanvas;
        [SerializeField] private UpperPanel _upperPanel;

        public override void InstallBindings()
        {
            Container.Bind<MainCanvas>().FromInstance(_mainCanvas).AsSingle().NonLazy();
            Container.Bind<UpperPanel>().FromInstance(_upperPanel).AsSingle().NonLazy();
        }
    }
}
