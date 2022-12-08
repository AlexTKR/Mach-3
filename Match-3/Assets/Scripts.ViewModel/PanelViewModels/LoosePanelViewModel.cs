using System;
using Scripts.CommonBehaviours;
using UnityWeld.Binding;

namespace Scripts.ViewModel.PanelViewModels
{
    public interface ILoosePanelBehaviour : ISetActiveStatus
    {
        event Action OnAdButtonClicked;
        event Action OnQuitButtonClicked;
    }

    [Binding]
    public class LoosePanelViewModel : ViewModelBase, ILoosePanelBehaviour
    {
        public event Action OnAdButtonClicked;
        public event Action OnQuitButtonClicked;

        [Binding]
        public void ProcessAdButtonClicked()
        {
            OnAdButtonClicked?.Invoke();
        }
        
        [Binding]
        public void ProcessQuitButtonClicked()
        {
            OnQuitButtonClicked?.Invoke();
        }

        public void SetStatus(bool status)
        {
            SetActiveStatus(status);
        }
    }
}
