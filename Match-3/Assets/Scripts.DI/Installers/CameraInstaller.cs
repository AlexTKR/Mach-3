using UnityEngine;
using View;
using Zenject;

namespace Scripts.Main.Installers
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private MainCamera _mainCamera;
        
        public override void InstallBindings()
        {
            Container.Bind<MainCamera>().FromInstance(_mainCamera).AsSingle().NonLazy();
        }
    }
}