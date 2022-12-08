using Scripts.Main.Controllers;
using Scripts.Main.Loadable;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Scripts.Composition.Root
{
    public class MapSceneRoot : RootBase
    {
        private ILoadMap _loadMap;
        private IGetEventSystem _getEventSystem;
        
        [Inject]
        private void Construct(ILoadMap loadMap, IGetEventSystem getEventSystem)
        {
            _loadMap = loadMap;
            _getEventSystem = getEventSystem;
        }

        protected override async void OnStart()
        {
#if UNITY_EDITOR
            if (FindObjectOfType<EventSystem>() is null)
            {
                var eventSystem = await _getEventSystem.GetEventSystem().Load();
                DontDestroyOnLoad(MonoBehaviour.Instantiate(eventSystem));
            }
#endif
            
            _loadMap.LoadMap();   
        }
    }
}
