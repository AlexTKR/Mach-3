using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.CommonBehaviours;
using Scripts.Main.Level;
using Scripts.Main.Loadable;
using Scripts.Main.UtilitiesAndHelpers;
using Scripts.ViewModel;
using Scripts.ViewModel.PanelViewModels;
using UnityEngine.U2D;
using Zenject;

namespace Scripts.Main.Controllers
{
    public interface IProcessMoves
    {
        void SetMovesCount(int count);
    }

    public interface IGoalBehaviour
    {
        void UpdateGoal(CellType type, int count);
        void SetGoal(LevelGoal[] goals);
    }

    public interface ILevelNumber
    {
        void SetLevelNumber(int number);
    }

    public interface IMainHudProcessor : IProcessMoves, IGoalBehaviour, ILevelNumber
    {
    }

    public interface ILoosePanel
    {
        void ProcessLoose();
    }

    public interface IWinPanel
    {
        void ProcessWin();
    }

    public class MainSceneUIController : ControllerBase, IMainHudProcessor, ILoosePanel,
        IWinPanel
    {
        private IMainHudViewModel _mainHudViewModel;
        private IWinPanelBehaviour _winPanelBehaviour;
        private ILoosePanelBehaviour _loosePanelBehaviour;
        private IPausePanelViewModel _pausePanelViewModel;

        private ILoadScene _loadScene;
        private SpriteAtlas _goalAtlas;

        [Inject]
        void Construct(IMainHudViewModel mainHudViewModel, IGetCellAtlas getCellAtlas,
            IWinPanelBehaviour winPanelBehaviour, ILoosePanelBehaviour loosePanelBehaviour,
            IPausePanelViewModel pausePanelViewModel,  ILoadScene loadScene) 
        {
            _mainHudViewModel = mainHudViewModel;
            _winPanelBehaviour = winPanelBehaviour;
            _loosePanelBehaviour = loosePanelBehaviour;
            _pausePanelViewModel = pausePanelViewModel;
            _loadScene = loadScene;
            _goalAtlas = getCellAtlas.GetCellAtlas().Load(runAsync: false).Result;
        }

        public override Task Init()
        {
            _winPanelBehaviour.OnContinueButtonClicked += ProcessLevelQuitCallback;
            _loosePanelBehaviour.OnQuitButtonClicked += ProcessLevelQuitCallback;
            _pausePanelViewModel.OnQuitButtonClicked += ProcessLevelQuitCallback;
            return base.Init();
        }

        public void SetMovesCount(int count)
        {
            _mainHudViewModel.MovesCount.Value = count;
        }

        public void SetLevelNumber(int number)
        {
            _mainHudViewModel.LevelNumber.Value = number.ToString();
        }

        public void UpdateGoal(CellType type, int count)
        {
            _mainHudViewModel.UpdateGoal(type, count);
        }

        public void SetGoal(LevelGoal[] goals)
        {
            for (int i = 0; i < goals.Length; i++)
            {
                var goal = goals[i];
                _mainHudViewModel.SetGoal(goal.Type, goal.Count, _goalAtlas.GetSprite(Helpers.TypeToName[goal.Type]));
            }
        }

        void ProcessLevelQuitCallback()
        {
            _loadScene.LoadScene(SharedData.MapSceneIndex);
        }

        public void ProcessLoose()
        {
            _loosePanelBehaviour.SetStatus(true);
        }

        public void ProcessWin()
        {
            _winPanelBehaviour.SetStatus(true);
        }
    }
}