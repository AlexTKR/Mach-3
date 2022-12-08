using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Scripts.CommonExtensions.Scripts;
using UnityEngine;

namespace Scripts.ViewModel
{
    public abstract class ViewModelBase :  MonoBehaviour, INotifyPropertyChanged
    {
        protected Action<bool> OnActiveStatusChangedCallback;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected  void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SetActiveStatusChangedCallback(Action<bool> callBack)
        {
            OnActiveStatusChangedCallback = callBack;
        }

        public virtual void SetActiveStatus(bool status)
        {
            gameObject.SetActiveOptimize(status);
            OnActiveStatusChangedCallback?.Invoke(status);
        }
        
    }
}
