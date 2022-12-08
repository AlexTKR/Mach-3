using System;
using Scripts.CommonBehaviours;
using UnityWeld.Binding;

namespace Scripts.ViewModel.PanelViewModels
{
    public interface IPausePanelViewModel : ISetActiveStatus
    {
        event Action OnQuitButtonClicked;
    }
    
    [Binding]
    public class PausePanelViewModel : ViewModelBase, IPausePanelViewModel
    {
        public event Action OnQuitButtonClicked;
        
        [Binding]
        public void ProcessQuitButtonClicked()
        {
            OnQuitButtonClicked?.Invoke();
        }
        
        [Binding]
        public void ProcessResumeButtonClicked()
        {
            SetActiveStatus(false); 
        }

        public void SetStatus(bool status)
        {
            SetActiveStatus(status);
        }
    }
}
