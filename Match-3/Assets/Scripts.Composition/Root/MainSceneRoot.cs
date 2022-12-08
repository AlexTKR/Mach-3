using Controllers;
using DB;
using Scripts.Main;
using Scripts.Main.Controllers;
using Scripts.Main.DB;
using Scripts.Main.Loadable;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Scripts.Composition.Root
{
    public abstract class RootBase : MonoBehaviour
    {
        protected IInitiator _Initiator;
        protected IDatabaseDispatcher _databaseDispatcher;
        private IDatabase _database;

        [Inject]
        private void Construct(IInitiator initiator,
           IDatabaseDispatcher databaseDispatcher,
            IDatabase database)
        {
            _Initiator = initiator;
            _databaseDispatcher = databaseDispatcher;
            _database = database;
        }

        protected virtual async void Start()
        {
            if (_databaseDispatcher is { } && _database is { })
                await _databaseDispatcher.Dispatch(_database);

            if (_Initiator is { })
                await _Initiator.Init();
            
            OnStart();
        }

        protected abstract void OnStart();
    }

    public class MainSceneRoot : RootBase
    {
        private ILoadLevel _loadLevel;
        private IGetEventSystem _getEventSystem;

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