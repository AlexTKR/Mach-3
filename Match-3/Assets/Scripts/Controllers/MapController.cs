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

    public interface ISelectLevel
    {
        void SelectLevel(int levelNumber);
    }

    public class MapController : ControllerBase, IMapController, ISelectLevel
    {
        private IGetLevelSettings _getLevelSettings;
        private ILevelPanel _levelPanel;
        private ILoadScene _loadScene;
        private ISaveValue<int> _lastCompletedLevelNumber;
        private ISaveValue<int> _selectedLevel;

        [Inject]
        void Construct(IGetLevelSettings getLevelSettings, ILevelPanel levelPanel
            , ILoadScene loadScene)
        {
            _getLevelSettings = getLevelSettings;
            _levelPanel = levelPanel;
            _loadScene = loadScene;
        }

        public override Task InjectDatabase(IDatabase database)
        {
            _lastCompletedLevelNumber = new DatabaseValue<int>(database, SharedData.LastCompletedLevelId);
            _selectedLevel = new DatabaseValue<int>(database, SharedData.SelectedLevel);
            return base.InjectDatabase(database);
        }

        public async void LoadMap()
        {
            var levelData = await _getLevelSettings.GetLevelSettings().Load(autoRelease:false);
            var levelList = levelData.LevelData;

            for (int i = 0; i < levelList.Count; i++)
            {
                var currentLevelData = levelList[i];
                var levelNumber = currentLevelData.LevelNumber;
                _levelPanel.LoadPanel(levelNumber, levelNumber <= _lastCompletedLevelNumber.Value + 1);
            }
        }

        public void SelectLevel(int levelNumber)
        {
            _selectedLevel.Save(levelNumber);
            _loadScene.LoadScene(SharedData.MainSceneIndex);
        }
    }
}