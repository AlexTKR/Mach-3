using System.Linq;
using UtilitiesAndHelpers;
using Zenject;

namespace Controllers
{
    public interface ILoadGameSettings
    {
        void LoadGameSettings();
    }

    public interface ISetLevelNumber
    {
        public void SetLevelNumber(int number);
    }

    public interface IGetLevelData
    {
        LevelData GetCurrentLevelData();
    }

    public interface IGetLastCompletedLevelNumber
    {
        int GetLastCompletedLevelNumber();
    }

    public interface ISetLastCompletedLevelNumber
    {
        void SetLastCompletedLevelNumber(int value);
    }

    public interface ILastCompletedLevelNumber : IGetLastCompletedLevelNumber,
        ISetLastCompletedLevelNumber
    {
    }

    public class GameController : ILoadGameSettings, IGetLevelData, ISetLevelNumber, ILastCompletedLevelNumber
    {
        private IGetGameSettings _getGameSettings;
        private IGetLevelSettings _getLevelSettings;
        private int _lastCompletedLevelNumber;

        private int _currentLevelNumber;

        [Inject]
        void Construct(IGetGameSettings getGameSettings, IGetLevelSettings getLevelSettings)
        {
            _getGameSettings = getGameSettings;
            _getLevelSettings = getLevelSettings;
        }

        public void LoadGameSettings()
        {
            SharedData.GameSettings = _getGameSettings.GetGameSettings();
        }

        public LevelData GetCurrentLevelData()
        {
            return _currentLevelNumber == 0
                ? null
                : _getLevelSettings.GetLevelSettings().LevelData
                    .First(data => data.LevelNumber == _currentLevelNumber);
        }

        public void SetLevelNumber(int number)
        {
            _currentLevelNumber = number;
        }

        public int GetLastCompletedLevelNumber() => _lastCompletedLevelNumber;

        public void  SetLastCompletedLevelNumber(int value)
        {
            if (_lastCompletedLevelNumber < value)
                _lastCompletedLevelNumber = value;
        }
    }
}