using System.Collections.Generic;
using System.Threading.Tasks;
using Level;
using UnityEngine;
using UnityEngine.U2D;
using UtilitiesAndHelpers;
using View;
using Zenject;

namespace Controllers
{
    public interface IMovesCounter
    {
        void SetMovesCount(int count);
    }

    public interface IGoalBehaviour
    {
        void ProcessGoal(CellType type, int count);
        void SetGoals(List<LevelGoal> goals);
    }

    public interface ILevelNumber
    {
        void SetLevelNumber(int number);
    }

    public interface IUpperPanelBehaviour : IMovesCounter, IGoalBehaviour, ILevelNumber
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

    public class MainSceneUIController : ControllerBase, IUpperPanelBehaviour, ILoosePanel,
        IWinPanel
    {
        private ILoadScene _loadScene;
        private UpperPanel _upperPanel;
        private PausePanel _pausePanel;
        private LoosePanel _loosePanel;
        private WinPanel _winPanel;

        private Dictionary<CellType, GoalView> _goalViews =
            new();

        private SpriteAtlas goalAtlas;
        private GoalView _levelGoalPrefab;

        [Inject]
        void Construct(UpperPanel upperPanel, PausePanel pausePanel, LoosePanel loosePanel,
            WinPanel winPanel, IGetLevelGoal getLevelGoal, IGetCellAtlas getCellAtlas, 
            ILoadScene loadScene)
        {
            _loadScene = loadScene;
            _upperPanel = upperPanel;
            _pausePanel = pausePanel;
            _loosePanel = loosePanel;
            _winPanel = winPanel;
            _levelGoalPrefab = getLevelGoal.GetLevelGoalPrefab();
            goalAtlas = getCellAtlas.GetCellAtlas();
        }

        public override Task Initialize()
        {
            _pausePanel.SetQuitCallback(ProcessLevelQuitCallback);
            _winPanel.SetQuitCallback(ProcessLevelQuitCallback);
            _loosePanel.SetQuitCallback(ProcessLevelQuitCallback);
            return base.Initialize();
        }

        public void SetMovesCount(int count)
        {
            _upperPanel.SetMovesCount(count);
        }

        public void SetLevelNumber(int number)
        {
            _upperPanel.SetLevelNumber(number);
        }

        public void ProcessGoal(CellType type, int count)
        {
            if (!_goalViews.ContainsKey(type))
                return;

            _goalViews[type].SetCount(count);
        }

        public void SetGoals(List<LevelGoal> goals)
        {
            for (int i = 0; i < goals.Count; i++)
            {
                var currentGoalData = goals[i];
                var currentGoal = MonoBehaviour.Instantiate(_levelGoalPrefab, _upperPanel.GoalContent);
                var goalType = currentGoalData.Type;
                currentGoal.SetImage(goalAtlas.GetSprite(Helpers.TypeToName[goalType]));
                currentGoal.SetCount(currentGoalData.Count);
                _goalViews[goalType] = currentGoal;
            }
        }

        void ProcessLevelQuitCallback()
        {
            _loadScene.LoadScene(SharedData.MapSceneIndex);
        }

        public void ProcessLoose()
        {
            _loosePanel.Enable();
        }

        public void ProcessWin()
        {
            _winPanel.Enable();
        }
    }
}