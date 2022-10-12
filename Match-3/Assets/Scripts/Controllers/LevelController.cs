using System;
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
        void LoadLevel(LevelData levelData);
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
        private Dictionary<CellType, int> _goals = new();
        private Action _onGoalAccomplished;
        private Action _onMovesEnd;

        private int _levelNumber;
        private int _movesCount;

        public LevelInstance(LevelData levelData, Action onGoalAccomplished, Action onMovesEnd)
        {
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

            if (_goals[cellType] <= 0)
                _goals.Remove(cellType);

            if (_goals.Count == 0)
                _onGoalAccomplished?.Invoke();
        }

        public void ProcessMove()
        {
            if (--_movesCount <= 0)
                _onMovesEnd?.Invoke();
        }
    }

    public class LevelController : ControllerBase, ILoadLevel, ILevelController
    {
        private IGetLevel _getLevel;
        private IInitGrid _initGrid;
        private ILevelInstance _levelInstance;

        [Inject]
        void Construct(IGetLevel getLevel)
        {
            _getLevel = getLevel;
        }

        public override void Tick()
        {
        }

        public void LoadLevel(LevelData levelData)
        {
            // var cells = _getLevel.GetLevel(levelData.LevelNumber);
            //
            // if (cells is null)
            //     return;
            //
            // _levelInstance = new LevelInstance(levelData,
            //     ProcessGoalAccomplished, ProcessMovesEnd);
            // _initGrid.GenerateGrid(cells);
        }

        public void ProcessMove()
        {
            _levelInstance.ProcessMove();
        }

        public void ProcessMatch(CellType cellType)
        {
            _levelInstance.ProcessMatch(cellType);
        }

        private void ProcessGoalAccomplished()
        {
        }

        private void ProcessMovesEnd()
        {
        }
    }
}