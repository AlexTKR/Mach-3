using System;
using Scripts.CommonBehaviours;
using UnityWeld.Binding;

namespace Scripts.ViewModel.PanelViewModels
{
    public interface IWinPanelBehaviour : ISetActiveStatus
    {
        event Action OnContinueButtonClicked;
    }

    [Binding]
    public class WinPanelViewModel : ViewModelBase, IWinPanelBehaviour
    {
        public event Action OnContinueButtonClicked;

        [Binding]
        public void ProcessContinueButtonClicked()
        {
            OnContinueButtonClicked?.Invoke();
        }

        public void SetStatus(bool status)
        {
            SetActiveStatus(status);
        }
    }
}
