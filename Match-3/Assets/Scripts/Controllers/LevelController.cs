using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using Zenject;

namespace Controllers
{
    [Serializable]
    public class LevelGoal
    {
        public CellType Type;
        public int Count;
    }

    [Serializable]
    public class LevelData
    {
        public int LevelNumber;
        public int Moves;
        public List<LevelGoal> LevelGoals;
    }

    public interface ILoadLevel<T>
    {
        void LoadLevel(T level);
    }

    public interface IProcessMove
    {
        void ProcessMove();
    }

    public interface IProcessMatch
    {
        void ProcessMatch(CellType cellType);
    }

    public interface ILevelInstance : IProcessMove, IProcessMatch
    {
    }

    public interface ILevelController : IProcessMove, IProcessMatch
    {
    }

    public class LevelInstance : ILevelInstance
    {
        private IMovesCounter _movesCounter;
        private IGoalBehaviour _goalBehaviour;
        private Dictionary<CellType, int> _goals = new();
        private Action<int> _onGoalAccomplished;
        private Action _onMovesEnd;

        private int _levelNumber;
        private int _movesCount;

        public LevelInstance(LevelData levelData, Action<int> onGoalAccomplished,
            Action onMovesEnd, IMovesCounter movesCounter, IGoalBehaviour goalBehaviour)
        {
            _movesCounter = movesCounter;
            _goalBehaviour = goalBehaviour;
            _onGoalAccomplished += onGoalAccomplished;
            _onMovesEnd += onMovesEnd;

            var levelGoals = levelData.LevelGoals;
            _levelNumber = levelData.LevelNumber;
            _movesCount = levelData.Moves;

            for (int i = 0; i < levelGoals.Count; i++)
            {
                var currentLevelGoal = levelGoals[i];
                _goals[currentLevelGoal.Type] = currentLevelGoal.Count;
            }
        }

        public void ProcessMatch(CellType cellType)
        {
            if (!_goals.ContainsKey(cellType))
                return;

            _goals[cellType]--;
            _goalBehaviour.ProcessGoal(cellType, _goals[cellType]);

            if (_goals[cellType] <= 0)
                _goals.Remove(cellType);

            if (_goals.Count == 0)
                _onGoalAccomplished?.Invoke(_levelNumber);
        }

        public void ProcessMove()
        {
            if (--_movesCount <= 0)
                _onMovesEnd?.Invoke();

            _movesCounter.SetMovesCount(_movesCount);
        }
    }

    public class LevelController : ControllerBase, ILoadLevel<LevelData>, ILevelController
    {
        private IGetLevel _getLevel;
        private IInitGrid _initGrid;
        private ILevelInstance _levelInstance;
        private IGetLevelSettings _getLevelSettings;
        private IUpperPanelBehaviour _upperPanelBehaviour;
        private ILoosePanel _loosePanel;
        private IWinPanel _winPanel;
        private ISetLastCompletedLevelNumber _setLastCompletedLevelNumber;

        [Inject]
        void Construct(IGetLevel getLevel, IInitGrid initGrid,
            IGetLevelSettings getLevelSettings, IUpperPanelBehaviour upperPanelBehaviour,
            ILoosePanel loosePanel, IWinPanel winPanel, 
            ISetLastCompletedLevelNumber setLastCompletedLevelNumber)
        {
            _getLevel = getLevel;
            _initGrid = initGrid;
            _loosePanel = loosePanel;
            _winPanel = winPanel;
            _getLevelSettings = getLevelSettings;
            _upperPanelBehaviour = upperPanelBehaviour;
            _setLastCompletedLevelNumber = setLastCompletedLevelNumber;
        }

        public void LoadLevel(LevelData levelData)
        {
#if UNITY_EDITOR
            levelData ??= _getLevelSettings.GetLevelSettings().LevelData.First(data => data.LevelNumber == 1);
#endif
            var cells = _getLevel.GetLevel(levelData.LevelNumber);

            if (cells is null)
                return;

            _levelInstance = new LevelInstance(levelData,
                ProcessGoalAccomplished, ProcessMovesEnd, _upperPanelBehaviour,
                _upperPanelBehaviour);
            _upperPanelBehaviour.SetLevelNumber(levelData.LevelNumber);
            _upperPanelBehaviour.SetMovesCount(levelData.Moves);
            _upperPanelBehaviour.SetGoals(levelData.LevelGoals);
            _initGrid.GenerateGrid(cells);
        }

        public void ProcessMove()
        {
            _levelInstance.ProcessMove();
        }

        public void ProcessMatch(CellType cellType)
        {
            _levelInstance.ProcessMatch(cellType);
        }

        private void ProcessGoalAccomplished(int levelNumber)
        {
            _setLastCompletedLevelNumber.SetLastCompletedLevelNumber(levelNumber);
            _winPanel.ProcessWin();
        }

        private void ProcessMovesEnd()
        {
            _loosePanel.ProcessLoose();
        }
    }
}