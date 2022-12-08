using System.Threading.Tasks;
using Scripts.CommonBehaviours;
using Scripts.Reactive;
using Scripts.ViewModel.ElementViewModels;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

namespace Scripts.ViewModel.PanelViewModels
{
    public interface IMainHudViewModel : IInit
    {
        IReactiveValue<int> MovesCount { get; set; }
        IReactiveValue<string> LevelNumber { get; set; }
        void SetGoal(CellType cellType, int count, Sprite icon);
        void UpdateGoal(CellType cellType, int count);
    }

    [Binding]
    public class MainHudViewModel : ViewModelBase, IMainHudViewModel
    {
        private IPausePanelViewModel _pausePanelViewModel;

        [Binding] public IReactiveValue<int> MovesCount { get; set; } = new ReactiveValue<int>();
        [Binding] public IReactiveValue<string> LevelNumber { get; set; } = new ReactiveString("Level number {0}");

        [Binding]
        public ObservableList<GoalViewViewModel> LevelGoals { get;} =
            new ObservableList<GoalViewViewModel>();

        [Binding]
        public void ProcessPauseButtonClicked()
        {
            _pausePanelViewModel?.SetStatus(true);
        }

        [Inject]
        private void Construct(IPausePanelViewModel pausePanelViewModel)
        {
            _pausePanelViewModel = pausePanelViewModel;
        }
        
        public Task Init()
        {
            MovesCount.OnChanged += () => OnPropertyChanged(nameof(MovesCount));
            LevelNumber.OnChanged += () => OnPropertyChanged(nameof(LevelNumber));
            return Task.CompletedTask;
        }

        public void SetGoal(CellType cellType, int count, Sprite icon)
        {
            LevelGoals.Add(new GoalViewViewModel(cellType, count, icon));
        }

        public void UpdateGoal(CellType cellType, int count)
        {
            for (int i = 0; i < LevelGoals.Count; i++)
            {
                var levelGoal = LevelGoals[i];
                if (levelGoal.CellType == cellType)
                {
                    levelGoal.GoalCount.Value = count;
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            MovesCount.Destroy();
            LevelNumber.Destroy();
        }
    }
}