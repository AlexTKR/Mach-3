using Level;
using Settings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using View;

namespace Controllers
{
    public interface IGetCellAtlas
    {
        SpriteAtlas GetCellAtlas();
    }

    public interface IGetCell
    {
        CellView GetCellPrefab();
    }

    public interface IGetGameSettings
    {
        GameSettings GetGameSettings();
    }

    public interface IGetLevelSettings
    {
        IGetLevelData GetLevelSettings();
    }

    public interface IGetLevelPanel
    {
        LevelPanel GetLeft();
        LevelPanel GetRight();
    }

    public interface IGetEventSystem
    {
        EventSystem GetEventSystem();
    }

    public interface IGetLevelGoal
    {
        GoalView GetLevelGoalPrefab();
    }

    public class BundleController : IGetLevel, IGetCellAtlas, IGetCell, IGetGameSettings,
        IGetLevelSettings, IGetLevelPanel, IGetEventSystem, IGetLevelGoal
    {
        private IGetLevel _getLevel;
        private SpriteAtlas _spriteAtlas;
        private CellView _cellView;
        private GameSettings _gameSettings;
        private Settings.IGetLevelData _levelSettings;
        private LevelPanel _leftLevelPanel;
        private LevelPanel _rightLevelPanel;
        private EventSystem _eventSystem;
        private GoalView _levelGoalPrefab;

        public CellType[,] GetLevel(int number)
        {
            _getLevel ??= Resources.Load<LevelHolder>("Level/LevelHolder");

            return _getLevel.GetLevel(number);
        }

        public SpriteAtlas GetCellAtlas()
        {
            return _spriteAtlas ??= Resources.Load<SpriteAtlas>("Atlases/CellAtlas");
            ;
        }

        public CellView GetCellPrefab()
        {
            return _cellView ??= Resources.Load<CellView>("Cells/Cell");
        }

        public GameSettings GetGameSettings()
        {
            return _gameSettings ??= Resources.Load<GameSettings>("Settings/GameSettings");
        }

        public IGetLevelData GetLevelSettings()
        {
            return _levelSettings ??= Resources.Load<LevelSettings>("Settings/LevelSettings");
        }

        public LevelPanel GetLeft()
        {
            return _leftLevelPanel ??= Resources.Load<LevelPanel>("Level/LeftLevelPanel");
        }

        public LevelPanel GetRight()
        {
            return _rightLevelPanel ??= Resources.Load<LevelPanel>("Level/RightLevelPanel");
        }

        public EventSystem GetEventSystem()
        {
            return _eventSystem ??= Resources.Load<EventSystem>("EventSystem/EventSystem");
        }

        public GoalView GetLevelGoalPrefab()
        {
            return _levelGoalPrefab ??= Resources.Load<GoalView>("Level/GoalImage");
        }
    }
}