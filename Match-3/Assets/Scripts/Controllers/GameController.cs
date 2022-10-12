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

    public interface ILevelData : ISetLevelNumber
    {
        LevelData GetCurrentLevelData();
    }

    public class GameController : ILoadGameSettings, ILevelData
    {
        private IGetGameSettings _getGameSettings;
        private int _currentLevelNumber;
        
        [Inject]
        void Construct(IGetGameSettings getGameSettings)
        {
            _getGameSettings = getGameSettings;
        }

        public void LoadGameSettings()
        {
            SharedData.GameSettings = _getGameSettings.GetGameSettings();
        }

        public LevelData GetCurrentLevelData()
        {
            return null;
        }

        public void SetLevelNumber(int number)
        {
            _currentLevelNumber = number;
        }
    }
}
