using Composition;
using Controllers;
using DB;
using UnityEngine;
using UnityEngine.EventSystems;
using View;
using Zenject;

namespace Root
{
    public abstract class RootBase : MonoBehaviour
    {
        protected IInitiator _Initiator;
        protected IDatabaseDispatcher _databaseDispatcher;
        protected ISetPanelProcessor _setPanelProcessor;
        protected IPanelProcessor _panelProcessor;
        private IDatabase _database;

        [Inject]
        private void Construct(IInitiator initiator, ISetPanelProcessor setPanelProcessor,
            IPanelProcessor panelProcessor, IDatabaseDispatcher databaseDispatcher,
            IDatabase database)
        {
            _Initiator = initiator;
            _setPanelProcessor = setPanelProcessor;
            _panelProcessor = panelProcessor;
            _databaseDispatcher = databaseDispatcher;
            _database = database;
        }

        protected async void Awake()
        {
            if (_Initiator is null)
                return;

            await _Initiator.InitViews();
        }

        protected virtual async void Start()
        {
            _setPanelProcessor.SetProcessor(_panelProcessor);

            if (_databaseDispatcher is { } && _database is { })
                await _databaseDispatcher.Dispatch(_database);
            
            if (_Initiator is {})
                await _Initiator.InitControllers();
        }
    }

    public class MainSceneRoot : RootBase
    {
        private ILoadLevel<LevelData> _loadLevel;
        private IGetLevelData _getLevelData;
        private IGetEventSystem _getEventSystem;
        private MainCanvas _mainCanvas;

        [Inject]
        private void Construct(ILoadLevel<LevelData> loadLevel, 
            IGetLevelData getLevelData, IGetEventSystem getEventSystem)
        {
            _loadLevel = loadLevel;
            _getLevelData = getLevelData;
            _getEventSystem = getEventSystem;
        }

        protected override void Start()
        {
#if UNITY_EDITOR

            if (FindObjectOfType<EventSystem>() is null)
                DontDestroyOnLoad(MonoBehaviour.Instantiate(_getEventSystem.GetEventSystem()));
#endif

            base.Start();
            _loadLevel.LoadLevel(_getLevelData.GetCurrentLevelData());
        }
    }
}