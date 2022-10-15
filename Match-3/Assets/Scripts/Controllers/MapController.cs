using System.Threading.Tasks;
using DB;
using UtilitiesAndHelpers;
using Zenject;

namespace Controllers
{
    public interface IMapController
    {
        void LoadMap();
    }

    public class MapController : ControllerBase, IMapController, ILoadLevel<int>
    {
        private IGetLevelSettings _getLevelSettings;
        private ILevelPanel _levelPanel;
        private ISetLevelNumber _setLevelNumber;
        private ILoadScene _loadScene;
        private DatabaseValue<int> _lastCompletedLevelNumber;

        [Inject]
        void Construct(IGetLevelSettings getLevelSettings, ILevelPanel levelPanel,
            ISetLevelNumber setLevelNumber, ILoadScene loadScene)
        {
            _getLevelSettings = getLevelSettings;
            _levelPanel = levelPanel;
            _setLevelNumber = setLevelNumber;
            _loadScene = loadScene;
        }

        public override Task InjectDatabase(IDatabase database)
        {
            _lastCompletedLevelNumber = new DatabaseValue<int>(database, SharedData.LastCompletedLevelId);
            return base.InjectDatabase(database);
        }

        public void LoadMap()
        {
            var levelData = _getLevelSettings.GetLevelSettings().LevelData;

            for (int i = 0; i < levelData.Count; i++)
            {
                var currentLevelData = levelData[i];
                var levelNumber = currentLevelData.LevelNumber;
                _levelPanel.LoadPanel(levelNumber, levelNumber <= _lastCompletedLevelNumber.Value + 1);
            }
        }

        public void LoadLevel(int levelNumber)
        {
            _setLevelNumber.SetLevelNumber(levelNumber);
            _loadScene.LoadScene(SharedData.MainSceneIndex);
        }
    }
}