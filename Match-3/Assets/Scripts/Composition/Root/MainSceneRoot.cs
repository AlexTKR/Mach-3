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
            if (_Initiator is { })
                await _Initiator.InitViews();
        }

        protected virtual async void Start()
        {
            _setPanelProcessor.SetProcessor(_panelProcessor);

            if (_databaseDispatcher is { } && _database is { })
                await _databaseDispatcher.Dispatch(_database);

            if (_Initiator is { })
                await _Initiator.InitControllers();
            
            OnStart();
        }

        protected abstract void OnStart();
    }

    public class MainSceneRoot : RootBase
    {
        private ILoadLevel _loadLevel;
        private IGetEventSystem _getEventSystem;
        private MainCanvas _mainCanvas;

        [Inject]
        private void Construct(ILoadLevel loadLevel,
            IGetEventSystem getEventSystem)
        {
            _loadLevel = loadLevel;
            _getEventSystem = getEventSystem;
        }

        protected override async void Start()
        {
#if UNITY_EDITOR
            if (FindObjectOfType<EventSystem>() is null)
            {
                var eventSystem = await _getEventSystem.GetEventSystem().Load();
                DontDestroyOnLoad(MonoBehaviour.Instantiate(eventSystem));
                _getEventSystem.GetEventSystem().Release();
            }
#endif

            base.Start();
        }

        protected override void OnStart()
        {
            _loadLevel.LoadLevel();
        }
    }
}