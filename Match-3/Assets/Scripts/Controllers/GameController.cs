using UtilitiesAndHelpers;
using Zenject;

namespace Controllers
{
    public interface ILoadGameSettings
    {
        void LoadGameSettings();
    }

    public class GameController : ControllerBase, ILoadGameSettings
    {
        private iGetGameSettings _getGameSettings;
        
        [Inject]
        void Construct(iGetGameSettings getGameSettings)
        {
            _getGameSettings = getGameSettings;
        }

        public void LoadGameSettings()
        {
            SharedData.GameSettings = _getGameSettings.GetGameSettings();
        }
    }
}
