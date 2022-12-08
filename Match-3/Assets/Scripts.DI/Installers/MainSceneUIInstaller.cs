using Scripts.ViewModel;
using Scripts.ViewModel.CanvasViewModels;
using UnityEngine;
using Zenject;

namespace Scripts.DI.Installers
{
    public class MainSceneUIInstaller : MonoInstaller
    {
        [SerializeField] private CanvasViewModelBase _mainCanvasViewModel;

        public override void InstallBindings()
        {
            BindViewModels(_mainCanvasViewModel.ViewModels);
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