using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB;
using Level;
using UnityEditor;
using UtilitiesAndHelpers;
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

    public interface ILoadLevel
    {
        void LoadLevel();
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

    public class LevelController : ControllerBase, ILoadLevel, ILevelController
    {
        private IGetLevel _getLevel;
        private IGridBehaviour _gridBehaviour;
        private ILevelInstance _levelInstance;
        private IGetLevelSettings _getLevelSettings;
        private IUpperPanelBehaviour _upperPanelBehaviour;
        private ILoosePanel _loosePanel;
        private IWinPanel _winPanel;
        private ISaveValue<int> _lastCompletedLevelNumber;
        private ISaveValue<int> _selectedLevel;

        [Inject]
        void Construct(IGetLevel getLevel, IGridBehaviour gridBehaviour,
            IGetLevelSettings getLevelSettings, IUpperPanelBehaviour upperPanelBehaviour,
            ILoosePanel loosePanel, IWinPanel winPanel)
        {
            _getLevel = getLevel;
            _gridBehaviour = gridBehaviour;
            _loosePanel = loosePanel;
            _winPanel = winPanel;
            _getLevelSettings = getLevelSettings;
            _upperPanelBehaviour = upperPanelBehaviour;
        }

        public override Task InjectDatabase(IDatabase database)
        {
            _lastCompletedLevelNumber = new DatabaseValue<int>(database, SharedData.LastCompletedLevelId);
            _selectedLevel = new DatabaseValue<int>(database, SharedData.SelectedLevel);
            return base.InjectDatabase(database);
        }

        public async void LoadLevel()
        {
            var levelData = await _getLevelSettings.GetLevelSettings().Load(autoRelease:false);
            var currentData = levelData.LevelData
                .First(data => data.LevelNumber == _selectedLevel.Value);
            var getCells = await _getLevel.GetCells().Load();
            var cells = getCells.GetCells(currentData.LevelNumber);

            if (cells is null)
                return;

            _levelInstance = new LevelInstance(currentData,
                ProcessGoalAccomplished, ProcessMovesEnd, _upperPanelBehaviour,
                _upperPanelBehaviour);
            _upperPanelBehaviour.SetLevelNumber(currentData.LevelNumber);
            _upperPanelBehaviour.SetMovesCount(currentData.Moves);
            _upperPanelBehaviour.SetGoals(currentData.LevelGoals);
            _gridBehaviour.GenerateGrid(cells);
        }

        public void ProcessMove()
        {
            _levelInstance.ProcessMove();
        }

        public void ProcessMatch(CellType cellType)
        {
            _levelInstance.ProcessMatch(cellType);
        }

        private async void ProcessGoalAccomplished(int levelNumber)
        {
            if (_lastCompletedLevelNumber.Value < levelNumber)
                _lastCompletedLevelNumber.Save(levelNumber);

            while (_gridBehaviour.IsActive)
            {
                await Task.Yield();
            }

            _winPanel.ProcessWin();
        }

        private void ProcessMovesEnd()
        {
            _loosePanel.ProcessLoose();
        }
    }
}