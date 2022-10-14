using UnityEngine;
using View;
using Zenject;

namespace Installers
{
    public class MainSceneUIInstaller : MonoInstaller
    {
        [SerializeField] private MainCanvas _mainCanvas;
        [SerializeField] private UpperPanel _upperPanel;
        [SerializeField] private PausePanel _pausePanel;
        [SerializeField] private WinPanel _winPanel;
        [SerializeField] private LoosePanel _loosePanel;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainCanvas>().FromInstance(_mainCanvas).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UpperPanel>().FromInstance(_upperPanel).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PausePanel>().FromInstance(_pausePanel).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WinPanel>().FromInstance(_winPanel).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LoosePanel>().FromInstance(_loosePanel).AsSingle().NonLazy();
        }
    }
}