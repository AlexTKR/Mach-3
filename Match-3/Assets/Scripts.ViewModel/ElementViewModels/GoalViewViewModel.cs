using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Scripts.CommonBehaviours;
using Scripts.Reactive;
using UnityEngine;
using UnityEngine.UI;
using UnityWeld.Binding;

namespace Scripts.ViewModel.ElementViewModels
{
    public interface IGoalViewBehaviour
    {
        public CellType CellType { get; }
        IReactiveValue<int> GoalCount { get; set; }
    }

    [Binding]
    public class GoalViewViewModel : INotifyPropertyChanged , IGoalViewBehaviour
    {
        public CellType CellType { get; }
        [Binding] public IReactiveValue<int> GoalCount { get; set; } = new ReactiveValue<int>();
        [Binding] public Sprite Icon { get; }

        public GoalViewViewModel(CellType cellType, int count, Sprite icon)
        {
            GoalCount.OnChanged += () => OnPropertyChanged(nameof(GoalCount));
            CellType = cellType;
            GoalCount.Value = count;
            Icon = icon;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}