using UtilitiesAndHelpers;
using Zenject;

namespace Controllers
{
    public interface IMapController
    {
        void LoadMap();
    }

    public class MapController : ControllerBase, IMapController , ILoadLevel<int>
    {
        private IGetLevelSettings _getLevelSettings;
        private ILevelPanel _levelPanel;
        private ISetLevelNumber _setLevelNumber;
        private ILoadScene _loadScene;
        private IGetLastCompletedLevelNumber _getLastCompletedLevelNumber;

        [Inject]
        void Construct(IGetLevelSettings getLevelSettings, ILevelPanel levelPanel,
            ISetLevelNumber setLevelNumber,  ILoadScene loadScene,
            IGetLastCompletedLevelNumber getLastCompletedLevelNumber)
        {
            _getLevelSettings = getLevelSettings;
            _levelPanel = levelPanel;
            _setLevelNumber = setLevelNumber;
            _loadScene = loadScene;
            _getLastCompletedLevelNumber = getLastCompletedLevelNumber;
        }

        public void LoadMap()
        {
            var levelData = _getLevelSettings.GetLevelSettings().LevelData;
            var lastCompletedLevel = _getLastCompletedLevelNumber.GetLastCompletedLevelNumber();

            for (int i = 0; i < levelData.Count; i++)
            {
                var currentLevelData = levelData[i];
                var levelNumber = currentLevelData.LevelNumber;
                _levelPanel.LoadPanel(levelNumber, levelNumber <= lastCompletedLevel + 1);
            }
        }

        public void LoadLevel(int levelNumber)
        {
            _setLevelNumber.SetLevelNumber(levelNumber);
            _loadScene.LoadScene(SharedData.MainSceneIndex);
        }
    }
}