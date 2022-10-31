using System.Threading.Tasks;
using Level;
using Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
//using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using View;

public interface ILoadable<T>
{
    Task<T> Load(bool autoRelease = true, bool runAsync = true);
    void Release();
}

public class LoadReference<T, TIn> : ILoadable<T>
{
    private string _id;
    private AsyncOperationHandle _handle;

    public LoadReference(string id)
    {
        _id = id;
    }

    public async Task<T> Load(bool autoRelease = true, bool runAsync = true)
    {
        _handle = Addressables.LoadAssetAsync<TIn>(_id);

        if (runAsync)
            await _handle.Task;
        else
            _handle.WaitForCompletion();

        var result = typeof(TIn) == typeof(GameObject)
            ? ((GameObject)_handle.Result).GetComponent<T>()
            : (T)_handle.Result;

        if (autoRelease)
        {
            Release();
        }

        return result;
    }

    public void Release()
    {
        //Addressables.Release(_handle);
    }
}

namespace Controllers
{
    public interface IGetCellAtlas
    {
        ILoadable<SpriteAtlas> GetCellAtlas();
    }

    public interface IGetCell
    {
        ILoadable<CellView> GetCellPrefab();
    }

    public interface IGetGameSettings
    {
        ILoadable<GameSettings> GetGameSettings();
    }

    public interface IGetLevelSettings
    {
        ILoadable<IGetLevelData> GetLevelSettings();
    }

    public interface IGetLevelPanel
    {
        ILoadable<LevelPanel> GetLeft();
        ILoadable<LevelPanel> GetRight();
    }

    public interface IGetEventSystem
    {
        ILoadable<EventSystem> GetEventSystem();
    }

    public interface IGetLevelGoalView
    {
        ILoadable<GoalView> GetLevelGoalPrefab();
    }

    public interface IGetLevel
    {
        ILoadable<IGetCells> GetCells();
    }

    public class BundleController : IGetLevel, IGetCellAtlas, IGetCell, IGetGameSettings,
        IGetLevelSettings, IGetLevelPanel, IGetEventSystem, IGetLevelGoalView
    {
        private ILoadable<IGetCells> _cells;
        private ILoadable<SpriteAtlas> _spriteAtlas;
        private ILoadable<CellView> _cellView;
        private ILoadable<GameSettings> _gameSettings;
        private ILoadable<IGetLevelData> _levelSettings;
        private ILoadable<LevelPanel> _leftLevelPanel;
        private ILoadable<LevelPanel> _rightLevelPanel;
        private ILoadable<EventSystem> _eventSystem;
        private ILoadable<GoalView> _levelGoalPrefab;

        public ILoadable<SpriteAtlas> GetCellAtlas()
        {
            return _spriteAtlas ??= new LoadReference<SpriteAtlas, SpriteAtlas>("CellAtlas");
        }

        public ILoadable<CellView> GetCellPrefab()
        {
            return _cellView ??= new LoadReference<CellView, GameObject>("Cell");
        }

        public ILoadable<GameSettings> GetGameSettings()
        {
            return new LoadReference<GameSettings, GameSettings>("GameSettings");
        }

        public ILoadable<IGetLevelData> GetLevelSettings()
        {
            return _levelSettings ??= new LoadReference<IGetLevelData, LevelSettings>("LevelSettings");
        }

        public ILoadable<LevelPanel> GetLeft()
        {
            return  _leftLevelPanel ??= new LoadReference<LevelPanel, GameObject>("LeftLevelPanel");
        }

        public ILoadable<LevelPanel> GetRight()
        {
            return _rightLevelPanel ??= new LoadReference<LevelPanel, GameObject>("RightLevelPanel");
        }

        public ILoadable<EventSystem> GetEventSystem()
        {
            return _eventSystem ??= new LoadReference<EventSystem, GameObject>("EventSystem");
        }

        public ILoadable<GoalView> GetLevelGoalPrefab()
        {
            return _levelGoalPrefab ??= new LoadReference<GoalView, GameObject>("GoalImage");
        }

        public ILoadable<IGetCells> GetCells()
        {
            return  _cells ??= new LoadReference<IGetCells, LevelHolder>("LevelHolder");
        }
    }
}