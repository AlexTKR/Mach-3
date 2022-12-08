using Scripts.ViewModel;
using Scripts.ViewModel.CanvasViewModels;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MapSceneUIInstaller :  MonoInstaller
    {
        [SerializeField] private CanvasViewModelBase _mapCanvasViewModel;
        
        public override void InstallBindings()
        {
            BindViewModels( _mapCanvasViewModel.ViewModels);
        }
        
        private void BindViewModels(ViewModelBase[] viewModels)
        {
            for (int i = 0; i < viewModels.Length; i++)
            {
                var viewModel = viewModels[i];
                Container.BindInterfacesTo(viewModel.GetType()).FromInstance(viewModel).NonLazy();
            }
        }
    }
}
