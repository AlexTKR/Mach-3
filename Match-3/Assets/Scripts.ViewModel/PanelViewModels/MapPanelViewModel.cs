using System;
using System.Threading.Tasks;
using Scripts.CommonBehaviours;
using UnityEngine;
using UnityWeld.Binding;

namespace Scripts.ViewModel.PanelViewModels
{
    public interface IMapPanelBehaviour : IInit
    {
        void LoadMapItem(int levelNumber, bool isActive, Action<int> onItemSelected);
    }

    [Binding]
    public class MapPanelViewModel : ViewModelBase, IMapPanelBehaviour
    {
        [SerializeField] private Color _itemActiveColor;
        [SerializeField] private Color _itemInactiveColor;

        private bool _leftItemAdded;

        [Binding]
        public ObservableList<MapItemViewModel> MapItemViewModels { get; } =
            new ObservableList<MapItemViewModel>();


        public Task Init()
        {
            return Task.CompletedTask;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < MapItemViewModels.Count; i++)
            {
                MapItemViewModels[i].Destroy();
            }
        }

        public void LoadMapItem(int levelNumber, bool isActive, Action<int> onItemSelected)
        {
            MapItemViewModel mapItemViewModel = _leftItemAdded
                ? new RightMapItemViewModel(levelNumber, isActive, onItemSelected,
                    isActive ? _itemActiveColor : _itemInactiveColor)
                : new LeftMapItemViewModel(levelNumber, isActive, onItemSelected,
                    isActive ? _itemActiveColor : _itemInactiveColor);
            _leftItemAdded = !_leftItemAdded;
            MapItemViewModels.Add(mapItemViewModel);
        }
    }
}