using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Scripts.CommonBehaviours;
using Scripts.Reactive;
using UnityEngine;
using UnityWeld.Binding;

namespace Scripts.ViewModel.PanelViewModels
{
    public interface IMapItemBehaviour: IDestroy
    {
        IReactiveValue<string> LevelNumberText { get; }
        IReactiveValue<bool> IsActive { get; }
    }


    [Binding]
    public class MapItemViewModel : INotifyPropertyChanged, IMapItemBehaviour
    {
        [Binding] public IReactiveValue<string> LevelNumberText { get; } = new ReactiveString("Level {0}");
        [Binding] public IReactiveValue<bool> IsActive { get; } = new ReactiveValue<bool>();
        [Binding] public IReactiveValue<int> SiblingIndex { get; } = new ReactiveValue<int>();
        [Binding] public IReactiveValue<Color> ItemColor { get; } = new ReactiveValue<Color>();

        private Action<int> _onItemSelected;
        private int _levelNumber;


        public MapItemViewModel(int levelNumber, bool isActive, Action<int> onItemSelected, Color color)
        {
            LevelNumberText.OnChanged += () => OnPropertyChanged(nameof(LevelNumberText));
            IsActive.OnChanged += () => OnPropertyChanged(nameof(IsActive));
            SiblingIndex.OnChanged += () => OnPropertyChanged(nameof(SiblingIndex));
            ItemColor.OnChanged += () => OnPropertyChanged(nameof(ItemColor));

            _levelNumber = levelNumber;
            LevelNumberText.Value = levelNumber.ToString();
            _onItemSelected += onItemSelected;
            IsActive.Value = isActive;
            ItemColor.Value = color;
            SiblingIndex.Value = 0;
        }

        [Binding]
        public void ProcessMapItemSelected()
        {
            _onItemSelected?.Invoke(_levelNumber);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Destroy()
        {
            LevelNumberText.Destroy();
            IsActive.Destroy();
            SiblingIndex.Destroy();
            ItemColor.Destroy();
        }
    }

    [Binding]
    public class LeftMapItemViewModel : MapItemViewModel
    {
        public LeftMapItemViewModel(int levelNumber, bool isActive, Action<int> onItemSelected, Color color) : base(
            levelNumber,
            isActive,
            onItemSelected, color)
        {
        }
    }

    [Binding]
    public class RightMapItemViewModel : MapItemViewModel
    {
        public RightMapItemViewModel(int levelNumber, bool isActive, Action<int> onItemSelected, Color color) : base(
            levelNumber,
            isActive, onItemSelected, color)
        {
        }
    }
}