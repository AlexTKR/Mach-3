using System.Linq;
using UtilitiesAndHelpers;
using Zenject;

namespace Controllers
{
    public interface ISetLevelNumber
    {
        public void SetLevelNumber(int number);
    }

    public interface IGetLevelData
    {
        LevelData GetCurrentLevelData();
    }

    public class GameController : IGetLevelData, ISetLevelNumber
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
            
            LoadGameSettings();
        }

        private void LoadGameSettings()
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
    }
}