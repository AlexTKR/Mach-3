using UnityEngine;
using View;
using Zenject;

namespace Installers
{
    public class MapSceneUIInstaller :  MonoInstaller
    {
        [SerializeField] private MapCanvas _mapCanvas;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MapCanvas>().FromInstance(_mapCanvas).AsSingle().NonLazy();
        }
    }
}
