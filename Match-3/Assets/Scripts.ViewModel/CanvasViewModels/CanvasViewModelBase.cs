using System;
using Scripts.CommonExtensions.Scripts;
using UnityEngine;

namespace Scripts.ViewModel.CanvasViewModels
{
    public abstract class CanvasViewModelBase : MonoBehaviour
    {
        [SerializeField] private GameObject _blockingPanel;
        public ViewModelBase[] ViewModels;

        public static bool AnyPanelActive = false;

        private void Awake()
        {
            AnyPanelActive = false;
            
            for (int i = 0; i < ViewModels.Length; i++)
            {
                var viewModel = ViewModels[i];
                viewModel.SetActiveStatusChangedCallback(activeStatus =>
                {
                    AnyPanelActive = activeStatus;
                    if (_blockingPanel != null && activeStatus &&
                        _blockingPanel.transform.parent != viewModel.transform)
                    {
                        _blockingPanel.transform.SetParent(viewModel.transform);
                        _blockingPanel.transform.SetAsFirstSibling();
                    }

                    _blockingPanel.SetActiveOptimize(activeStatus);
                });
            }
        }
    }
}