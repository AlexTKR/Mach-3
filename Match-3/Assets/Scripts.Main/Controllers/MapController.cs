using System.Threading.Tasks;
using DB;
using Scripts.Main.Loadable;
using Scripts.Main.UtilitiesAndHelpers;
using Zenject;

namespace Scripts.Main.Controllers
{
    public interface ILoadMap
    {
        void LoadMap();
    }

    public interface ISelectLevel
    {
        void SelectLevel(int levelNumber);
    }

    public class MapController : ControllerBase, ILoadMap, ISelectLevel
    {
        private IGetLevelSettings _getLevelSettings;
        private IloadMapItem _iLoadMapItem;
        private ILoadScene _loadScene;
        private ISaveValue<int> _lastCompletedLevelNumber;
        private ISaveValue<int> _selectedLevel;

        [Inject]
        void Construct(IGetLevelSettings getLevelSettings, IloadMapItem iLoadMapItem
            , ILoadScene loadScene)
        {
            _getLevelSettings = getLevelSettings;
            _iLoadMapItem = iLoadMapItem;
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
            var levelData = await _getLevelSettings.GetLevelSettings().Load(autoRelease: false);
            var levelList = levelData.LevelData;

            for (int i = 0; i < levelList.Count; i++)
            {
                var currentLevelData = levelList[i];
                var levelNumber = currentLevelData.LevelNumber;
                _iLoadMapItem.LoadMapItem(levelNumber, levelNumber <= _lastCompletedLevelNumber.Value + 1);
            }
        }

        public void SelectLevel(int levelNumber)
        {
            _selectedLevel.Save(levelNumber);
            _loadScene.LoadScene(SharedData.MainSceneIndex);
        }
    }
}