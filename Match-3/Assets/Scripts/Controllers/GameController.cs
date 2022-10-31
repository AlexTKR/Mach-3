using UtilitiesAndHelpers;
using Zenject;

namespace Controllers
{
    public class GameController 
    {
        private IGetGameSettings _getGameSettings;
        
        [Inject]
        void Construct(IGetGameSettings getGameSettings)
        {
            _getGameSettings = getGameSettings;

            LoadGameSettings();
        }

        private void LoadGameSettings()
        {
            SharedData.GameSettings = _getGameSettings.GetGameSettings().Load(runAsync: false).Result;
        }
    }
}